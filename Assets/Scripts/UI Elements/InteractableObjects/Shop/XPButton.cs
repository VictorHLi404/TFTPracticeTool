using UnityEngine;
public class XPButton : MonoBehaviour {

    private ShopUI parentShop;

    public XPButton(ShopUI _parentShop) {
        this.parentShop = _parentShop;
    }

    public void setParentShop(ShopUI _parentShop) {
        this.parentShop = _parentShop;
    }

    public void OnMouseDown() {
        parentShop.buyXP();
    }

}