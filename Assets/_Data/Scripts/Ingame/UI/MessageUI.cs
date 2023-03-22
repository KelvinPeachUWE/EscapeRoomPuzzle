using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] PlayerInteract playerInteract; // Which player should this message box display for?

    [Header("Cache")]
    [SerializeField] GameObject panel;
    [SerializeField] Text titleText;
    [SerializeField] Text messageText;

    void Awake()
    {
        // Subscribe to events
        playerInteract.onItemStartedLookingAt += OnStartedLookingAtItem;
        playerInteract.onItemStoppedLookingAt += OnStoppedLookingAtItem;
        playerInteract.onInteractableStartedLookingAt += OnStartedLookingAtInteractable;
        playerInteract.onInteractableStoppedLookingAt += OnStoppedLookingAtInteractable;
        playerInteract.onItemDestroyed += OnItemDestroyed;
        playerInteract.onHintStartedLookingAt += OnHintStartedLookingAt;
        playerInteract.onHintStoppedLookingAt += OnHintStoppedLookingAt;
        Keypad.onKeypadUsed += OnKeypadUsed;
        playerInteract.onItemPickedUp += OnItemPickedUp;
        playerInteract.onItemDropped += OnItemDropped;
    }

    void OnStartedLookingAtItem(ItemPickup itemStartedLookingAt)
    {
        // Set the message box's title to the item's name
        titleText.text = itemStartedLookingAt.name;
        
        // Set the main body text to how to pick it up
        messageText.text = "Press 'X' to pickup";

        // Show the message box
        Show();
    }

    void OnStartedLookingAtInteractable(Interactable interactableStartedLookingAt, ItemPickup heldItem)
    {
        // Set the message box's title to the interactable's name
        titleText.text = interactableStartedLookingAt.name;

        // Is an item required to use this interactable?
        if (interactableStartedLookingAt.RequiredItem)
        {
            // Does the player have the required item?
            if (heldItem && interactableStartedLookingAt.RequiredItem.name == heldItem.name)
            {
                // Let the player know what button to press to use the held item on the interactable
                messageText.text = "Press 'X' to use " + heldItem.name;
            }
            // The player doesn't have the required item
            else
            {
                // Let the player know what item they need to find
                messageText.text = interactableStartedLookingAt.RequiredItem.name + " required";
            }
        }
        else
        {
            // Let the player know what button to press to use the held item on the interactable
            messageText.text = "Press 'X' to use " + interactableStartedLookingAt.name;
        }

        // Show the message box
        Show();
    }

    void OnStoppedLookingAtItem(ItemPickup itemStoppedLookingAt)
    {
        // Hide the message box now nothing is being looked at
        Hide();
    }

    void OnStoppedLookingAtInteractable(Interactable interactableStoppedLookingAt)
    {
        // Hide the message box now nothing is being looked at
        Hide();
    }

    void OnItemDestroyed(ItemPickup itemDestroyed)
    {
        // Hide the message now an the held item has been destroyed
        Hide();
    }

    void OnKeypadUsed(Keypad keypad)
    {
        // Hide message box when using a keypad (we need the whole screen and it has its own mini message box)
        Hide();
    }

    void OnHintStartedLookingAt(Hint hint)
    {
        // Set the message box's title to the hint title
        titleText.text = hint.HintTitle;
        
        // Set the main body text to the actual hint
        messageText.text = hint.HintMessage;

        // Show the message box
        Show();
    }

    void OnHintStoppedLookingAt(Hint hint)
    {
        // Hide the message now a hint is no longer being looked at
        Hide();
    }

    void OnItemPickedUp(ItemPickup itemPickedUp)
    {
        titleText.text = itemPickedUp.name;
        messageText.text = "Press 'X' to throw " + itemPickedUp.name;
    }

    void OnItemDropped(ItemPickup itemDropped)
    {
        titleText.text = "";
        messageText.text = "";
    }

    void Show()
    {
        panel.SetActive(true);
    }

    void Hide()
    {
        panel.SetActive(false);
    }
}