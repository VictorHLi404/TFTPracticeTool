using UnityEngine;

public class RerollButton : MonoBehaviour {

    private ShopUI parentShop;

    public RerollButton(ShopUI _parentShop) {
        this.parentShop = _parentShop;
    }

    public void setParentShop(ShopUI _parentShop) {
        this.parentShop = _parentShop;
    }

    public void OnMouseDown() {
        parentShop.RerollShop();
    }

}