using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    public interface PermissionProvider
    {
        bool HasPermission();
    }
    private PermissionProvider permission;

    public interface InputReceiver
    {
        void OnHorizontalAxis(float horizontalAxis);
        void OnVerticalAxis(float verticalAxis);
        void OnJumpButtonDown();
    }

    private HashSet<InputReceiver> inputReceivers;
    private HashSet<InputReceiver> inputReceiverRegisterAwaiters;
    private HashSet<InputReceiver> inputReceiverUnregisterAwaiters;

    private void Awake()
    {
        inputReceivers = new HashSet<InputReceiver>();
        inputReceiverRegisterAwaiters = new HashSet<InputReceiver>();
        inputReceiverUnregisterAwaiters = new HashSet<InputReceiver>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputReceivers.UnionWith(inputReceiverRegisterAwaiters);
        inputReceiverRegisterAwaiters.Clear();
        inputReceivers.ExceptWith(inputReceiverUnregisterAwaiters);
        inputReceiverUnregisterAwaiters.Clear();

        if(permission != null && permission.HasPermission())
        {
            foreach(var receiver in inputReceivers)
            {
                receiver.OnHorizontalAxis(Input.GetAxis("Horizontal"));
                receiver.OnVerticalAxis(Input.GetAxis("Vertical"));
                
                if (Input.GetButtonDown("Jump"))
                    receiver.OnJumpButtonDown();
            }
        }
    }

    public void SetPermissionProvider(PermissionProvider newPermissionProvider)
    {
        this.permission = newPermissionProvider;
    }

    public void AddInputReceiverRegisterAwaiter(InputReceiver inputReceiver)
    {
        inputReceiverRegisterAwaiters.Add(inputReceiver);
    }

    public void AddInputReceiverUnregisterAwaiter(InputReceiver inputReceiver)
    {
        inputReceiverUnregisterAwaiters.Add(inputReceiver);
    }

}
