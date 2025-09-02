using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;
using TMPro;

public class BuyingSubscription : MonoBehaviour, IDetailedStoreListener
{
    private IStoreController store;
    private IExtensionProvider ext;

    [Header("Apple Store Product IDs (ASC ile birebir)")]
    public string classicProductId  = "classicpack.lovedare";
    public string standardProductId = "standardpack.lovedare";
    public string premiumProductId  = "premiumpack.lovedare";

    [Header("UI")]
    public TMP_Text subscriptionStatusText;
    public Button classicBtn, standardBtn, premiumBtn;
    public Text diagText; // opsiyonel log alanı

    // >>> EKLENENLER <<<
    [Header("LoveRoulette Lock")]
    public Button LoveRouletteBtn;     // kilitlenecek buton
    public GameObject LockImage;       // kilit görseli (üzerine ikon vb.)
    // <<< EKLENENLER >>>

    public bool HasActiveSubscription()
{
    if (store == null) return false;
    return IsSubscribedTo(store.products.WithID(premiumProductId)) ||
           IsSubscribedTo(store.products.WithID(standardProductId)) ||
           IsSubscribedTo(store.products.WithID(classicProductId));
}

    void Start()
    {
        SetButtons(false);
        Log("IAP init...");
        InitializePurchasing();

        // >>> EKLENEN: Oyuna kilitli başla (abonelik yok varsayımı, init sonrası açılacak)
        SetLoveRouletteLocked(true);
        // <<<
    }

    void SetButtons(bool v)
    {
        if (classicBtn)  classicBtn.interactable  = v;
        if (standardBtn) standardBtn.interactable = v;
        if (premiumBtn)  premiumBtn.interactable  = v;
    }

    // >>> EKLENEN: kilit helper
    void SetLoveRouletteLocked(bool locked)
    {
        if (LoveRouletteBtn) LoveRouletteBtn.interactable = !locked;
        if (LockImage) LockImage.SetActive(locked);
    }
    // <<<

    void InitializePurchasing()
    {
        var module = StandardPurchasingModule.Instance(AppStore.AppleAppStore);
        var builder = ConfigurationBuilder.Instance(module);

        builder.AddProduct(classicProductId,  ProductType.Subscription);
        builder.AddProduct(standardProductId, ProductType.Subscription);
        builder.AddProduct(premiumProductId,  ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    // UI onClick’lere bunları bağla
    public void BuyClassic()  => Buy(classicProductId);
    public void BuyStandard() => Buy(standardProductId);
    public void BuyPremium()  => Buy(premiumProductId);

    void Buy(string id)
    {
        if (store == null) { Log("Not initialized"); return; }
        var p = store.products.WithID(id);
        if (p == null) { Log("Product not found: " + id); return; }
        if (!p.availableToPurchase) { Log("Not available: " + id); return; }
        Log("InitiatePurchase: " + id);
        store.InitiatePurchase(p);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        store = controller; ext = extensions;
        Log("IAP initialized ✅");

        foreach (var p in store.products.all)
            Log($"id={p.definition.id} avail={p.availableToPurchase} price={p.metadata?.localizedPriceString}");

        bool any = false; foreach (var p in store.products.all) any |= p.availableToPurchase;
        SetButtons(any);
        UpdateUI(); // abonelik durumu geldiğinde kilidi günceller
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Log($"IAP init FAIL: {error} {message}");
        SetButtons(false);
        // init fail ise kilitli kalsın
        SetLoveRouletteLocked(true);
    }
    public void OnInitializeFailed(InitializationFailureReason error) => OnInitializeFailed(error, null);

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Log("Purchased: " + args.purchasedProduct.definition.id);
        UpdateUI(); // satın alma sonrası kilidi tekrar değerlendir
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Log($"Purchase FAIL: {product?.definition?.id} reason={failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Log($"Purchase FAIL: {product?.definition?.id} reason={failureDescription.reason} msg={failureDescription.message}");
    }

    bool IsSubscribedTo(Product subscription)
    {
        if (subscription == null || subscription.receipt == null) return false;
        try
        {
            var manager = new SubscriptionManager(subscription, null);
            var info = manager.getSubscriptionInfo();
            return info.isSubscribed() == Result.True;
        }
        catch (Exception e) { Log("SubInfo err: " + e.Message); return false; }
    }

    void UpdateUI()
    {
        if (store == null)
        {
            if (subscriptionStatusText) subscriptionStatusText.text = "";
            SetLoveRouletteLocked(true);
            return;
        }

        string status = "";
        if (IsSubscribedTo(store.products.WithID(premiumProductId)))       status = "PREMIUM";
        else if (IsSubscribedTo(store.products.WithID(standardProductId))) status = "STANDARD";
        else if (IsSubscribedTo(store.products.WithID(classicProductId)))  status = "CLASSIC";

        if (subscriptionStatusText) subscriptionStatusText.text = status;

        // >>> EKLENEN: abonelik yoksa kilit açık kalsın
        bool hasSub = !string.IsNullOrEmpty(status);
        SetLoveRouletteLocked(!hasSub);
        // <<<
    }

    void Log(string s) { Debug.Log(s); if (diagText) diagText.text = s; }
}
