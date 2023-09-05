using CoMZ2;
using UnityEngine;

public class UIShopIapPanelController : UIShopPanelController
{
	public TUIScrollerEx Scroller;

	public Transform ScrollerObjList;

	private float space = 100f;

	public override void Show()
	{
		base.Show();
		ChangeMask("Mask");
		if (ScrollerObjList.childCount <= 0)
		{
			for (IapCenter.IapIdName iapIdName = IapCenter.IapIdName.Cent99; iapIdName < IapCenter.IapIdName.Count; iapIdName++)
			{
				GameObject gameObject = Object.Instantiate(Resources.Load("Prefab/iapInfo")) as GameObject;
				gameObject.transform.parent = ScrollerObjList;
				gameObject.transform.localPosition = new Vector3(-174f + space * (float)iapIdName, 70f, 0f);
				gameObject.GetComponent<IapDataObj>().Init(iapIdName);
			}
			int num = -1;
			CreateExchangeObject(new CrystalExchangeCash
			{
				Crystal = 50,
				Cash = 100000,
				CashOrVoucher = true
			}, ++num);
			CreateExchangeObject(new CrystalExchangeCash
			{
				Crystal = 50,
				Voucher = 40
			}, ++num);
			CreateExchangeObject(new CrystalExchangeCash
			{
				Crystal = 500,
				Cash = 1600000,
				CashOrVoucher = true
			}, ++num);
			CreateExchangeObject(new CrystalExchangeCash
			{
				Crystal = 500,
				Voucher = 640
			}, ++num);
			CreateExchangeObject(new CrystalExchangeCash
			{
				Crystal = 2500,
				Cash = 12500000,
				CashOrVoucher = true
			}, ++num);
			CreateExchangeObject(new CrystalExchangeCash
			{
				Crystal = 2500,
				Voucher = 5000
			}, ++num);
			Scroller.borderXMin = 0f - (-174f + space * (float)num);
			Scroller.rangeXMin = Scroller.borderXMin;
		}
	}

	private void CreateExchangeObject(CrystalExchangeCash exchange, int index)
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("Prefab/iapInfo")) as GameObject;
		gameObject.transform.parent = ScrollerObjList;
		gameObject.transform.localPosition = new Vector3(-174f + space * (float)index, -70f, 0f);
		gameObject.GetComponent<IapDataObj>().Init(exchange);
	}

	private void OnTapJoyButton(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (eventType == 3)
		{
			Debug.Log("show tap joy.");
			TapjoyPlugin.ShowOffers();
		}
	}
}
