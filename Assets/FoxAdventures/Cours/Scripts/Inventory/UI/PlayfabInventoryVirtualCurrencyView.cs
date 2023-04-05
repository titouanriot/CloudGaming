using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabInventoryVirtualCurrencyView : MonoBehaviour
{
    void OnEnable()
    {
        // Hide
        this.Hide();

        // Inventory is setup?
        if (PlayfabInventory.Instance != null)
        {
            // Register to events
            PlayfabInventory.Instance.OnInventoryUpdateSuccess.AddListener(this.OnInventoryUpdateSuccess);
            PlayfabInventory.Instance.OnInventoryUpdateError.AddListener(this.OnInventoryUpdateError);

            // Ask inventory to update itself
            PlayfabInventory.Instance.UpdateInventory();
        }

        //// Update view to init
        this.UpdateView();
    }

    void OnDisable()
    {
        // Inventory is setup?
        if (PlayfabInventory.Instance != null)
        {
            // Unregister to events
            PlayfabInventory.Instance.OnInventoryUpdateSuccess.RemoveListener(this.OnInventoryUpdateSuccess);
            PlayfabInventory.Instance.OnInventoryUpdateError.RemoveListener(this.OnInventoryUpdateError);
        }
    }

    private void OnInventoryUpdateSuccess()
    {
        this.Show();
        this.UpdateView();
    }

    private void OnInventoryUpdateError()
    {
        this.Show();
        this.UpdateView();
    }

    [Header("Inventory")]
    public string virtualCurrencyListing = "DM";

    [Header("UI")]
    // Content root
    public Transform contentRoot = null;

    // Content UI
    public Text usernameText = null;
    //
    public Text crystalsCountText = null;
    public Image crystalsIcon = null;

    public void UpdateView()
    {
        int crystalsCount = 0;
        var request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request,
        success =>  
        {
            int crystalsCount = success.VirtualCurrency["DM"];
            if (PlayfabInventory.Instance != null && PlayfabInventory.Instance.VirtualCurrency != null && PlayfabInventory.Instance.VirtualCurrency.ContainsKey("DM") == true)
            {
                crystalsCount = PlayfabInventory.Instance.VirtualCurrency[this.virtualCurrencyListing];
            }

            // Update crystals count
            {
                if (this.crystalsCountText != null)
                {
                    this.crystalsCountText.gameObject.SetActive(true);
                    this.crystalsCountText.text = crystalsCount.ToString();
                }

                if (this.crystalsIcon != null)
                    this.crystalsIcon.gameObject.SetActive(true);
            }
        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });



        // Show
        this.Show();
    }

    // Show / Hide
    void Show()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(true);
    }

    void Hide()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(false);
    }
}
