using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class FoxCharacterWinScreenAddCrystals : FoxCharacterWinScreen
{
    protected override void OnWin()
    {
        // Data from the level we just finished
        int crystalsCount = this.FoxCharacterInventory.jewelsCount;

        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest
        {
            Amount = crystalsCount,
            VirtualCurrency = "DM"
        };

        PlayFabClientAPI.AddUserVirtualCurrency(request,
        res =>
        {
            Debug.Log("Ajout AddUserVirtualCurrency effectué");
        },
        err =>
        {
            Debug.Log("Erreur ajout monnaie virtuelle");
        });
       
        // Call base function from the class "FoxCharacterWinScreen" to display our score on the end screen & show buttons to go back to the Menu
        base.OnWin();
    }
}
