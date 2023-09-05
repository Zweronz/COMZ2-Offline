using UnityEngine;

public class TUIControlImpl : TUIControl
{
	public GameObject m_showClipObj;

	public GameObject m_hideClipObj;

	public Vector2 size = Vector2.zero;

	public void OnDrawGizmos()
	{
		float num = size.x / 2f;
		float num2 = size.y / 2f;
		Vector3[] array = new Vector3[4]
		{
			base.transform.TransformPoint(0f - num, num2, 0f),
			base.transform.TransformPoint(num, num2, 0f),
			base.transform.TransformPoint(num, 0f - num2, 0f),
			base.transform.TransformPoint(0f - num, 0f - num2, 0f)
		};
		Gizmos.color = Color.white;
		Gizmos.DrawLine(array[0], array[1]);
		Gizmos.DrawLine(array[1], array[2]);
		Gizmos.DrawLine(array[2], array[3]);
		Gizmos.DrawLine(array[3], array[0]);
		Gizmos.DrawLine(array[0], array[2]);
	}

	public void OnDrawGizmosSelected()
	{
		float num = size.x / 2f;
		float num2 = size.y / 2f;
		Vector3[] array = new Vector3[4]
		{
			base.transform.TransformPoint(0f - num, num2, 0f),
			base.transform.TransformPoint(num, num2, 0f),
			base.transform.TransformPoint(num, 0f - num2, 0f),
			base.transform.TransformPoint(0f - num, 0f - num2, 0f)
		};
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(array[0], array[1]);
		Gizmos.DrawLine(array[1], array[2]);
		Gizmos.DrawLine(array[2], array[3]);
		Gizmos.DrawLine(array[3], array[0]);
		Gizmos.DrawLine(array[0], array[2]);
	}

	public virtual bool PtInControl(Vector2 point)
	{
		float num = size.x / 2f;
		float num2 = size.y / 2f;
		Vector3[] array = new Vector3[4];
		Rect rect = default(Rect);
		rect.xMin = 0f - num;
		rect.xMax = num;
		rect.yMin = 0f - num2;
		rect.yMax = num2;
		if (null != m_showClipObj)
		{
			TUIRect component = m_showClipObj.GetComponent<TUIRect>();
			if (null != component)
			{
				Rect rectLocal = component.GetRectLocal(base.transform);
				Rect rect2 = TUIRect.RectIntersect(rectLocal, rect);
				rect = rect2;
			}
		}
		if (null != m_hideClipObj)
		{
			TUIRect component2 = m_hideClipObj.GetComponent<TUIRect>();
			if (null != component2)
			{
				Rect rectLocal2 = component2.GetRectLocal(base.transform);
				Rect rect3 = TUIRect.RectIntersect(rect, rectLocal2);
				array[0] = base.transform.TransformPoint(rect3.xMin, rect3.yMax, 0f);
				array[1] = base.transform.TransformPoint(rect3.xMax, rect3.yMax, 0f);
				array[2] = base.transform.TransformPoint(rect3.xMax, rect3.yMin, 0f);
				array[3] = base.transform.TransformPoint(rect3.xMin, rect3.yMin, 0f);
				if (PointInPolygon(array, point))
				{
					return false;
				}
			}
		}
		array[0] = base.transform.TransformPoint(rect.xMin, rect.yMax, 0f);
		array[1] = base.transform.TransformPoint(rect.xMax, rect.yMax, 0f);
		array[2] = base.transform.TransformPoint(rect.xMax, rect.yMin, 0f);
		array[3] = base.transform.TransformPoint(rect.xMin, rect.yMin, 0f);
		return PointInPolygon(array, point);
	}

	protected bool PointInPolygon(Vector3[] v, Vector2 point)
	{
		bool flag = false;
		int num = v.Length;
		for (int i = 0; i < num; i++)
		{
			if ((!(point.y < v[i].y) || !(point.y < v[(i + 1) % num].y)) && (!(v[i].x <= point.x) || !(v[(i + 1) % num].x <= point.x)))
			{
				float num2 = v[(i + 1) % num].x - v[i].x;
				float num3 = v[(i + 1) % num].y - v[i].y;
				float num4 = (point.x - v[i].x) / num2;
				float num5 = num4 * num3 + v[i].y;
				if (num5 <= point.y && num4 >= 0f && num4 <= 1f)
				{
					flag = !flag;
				}
			}
		}
		return flag;
	}
}
