using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UnityIAPManager : MonoBehaviour, IStoreListener {
    private IStoreController controller;

    public void Initialize () {
        Debug.Log ("UnityIAP.Init()");

        // var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        StandardPurchasingModule module = StandardPurchasingModule.Instance ();
        // ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();
        ConfigurationBuilder builder = ConfigurationBuilder.Instance (module);
        builder.AddProduct ("100.gold.coins", ProductType.Consumable);
        builder.AddProduct ("100.gold.coins.test", ProductType.Consumable);
        builder.AddProduct ("100.gold.coin.test0", ProductType.Consumable);
        builder.AddProduct ("100.gold.coin.test1", ProductType.Consumable);
        // IAPConfigurationHelper.PopulateConfigurationBuilder(ref builder, catalog);

        UnityPurchasing.Initialize (this, builder);
    }

    public void OnInitialized (IStoreController controller, IExtensionProvider extensions) {
        Debug.Log ("UnityIAP.OnInitialized Success");
        this.controller = controller;
    }

    public void OnInitializeFailed (InitializationFailureReason error) {
        Debug.Log ("UnityIAP.OnInitializeFailed(" + error + ")");

    }

    public void PurchaseProduct (string productId) {
        Debug.Log ("UnityIAP.BuyClicked(" + productId + ")");
        this.controller.InitiatePurchase (productId);
    }

    public void OnPurchaseFailed (UnityEngine.Purchasing.Product item, PurchaseFailureReason r) {
        Debug.Log ("UnityIAP.OnPurchaseFailed(" + item + ", " + r + ")");
    }

    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e) {
        string purchasedItem = e.purchasedProduct.definition.id;

        Debug.Log ("Puchase product: " + purchasedItem + "\n Result: Success");

        return PurchaseProcessingResult.Complete;
    }

}