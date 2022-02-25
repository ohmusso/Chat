using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] GameObject nodePrefab;
    [SerializeField] GameObject area;
    private GameObject node;
    private Vector2 areaSize;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitFirstFlameEnd());
        StartCoroutine(CreateNode());
    }

    private IEnumerator InitFirstFlameEnd()
    {
        yield return new WaitForEndOfFrame();
        areaSize = area.GetComponent<RectTransform>().rect.size;
        Debug.Log("area size: " + areaSize);
    }

    private Vector2 FitAreaPosAncUpLeft(Vector2 areaSize, Vector2 nodeSize, Vector2 v)
    {
        var ret = new Vector2(0, 0);

        Debug.Log("Random position: " + v);

        // upper left is (0, 0) on the area.
        float left = 0;
        float right =  areaSize.x - nodeSize.x; // node anchor is upper left ,so right side on the area need much space of node. 
        float upper = 0;
        float bottom = -(areaSize.y - nodeSize.y); // node anchor is upper left ,so bottom side on the area need much space of node. 

        ret.x = (v.x > left) ? v.x : left; 
        ret.x = (v.x < right) ? v.x : right; 

        ret.y = (v.y > bottom) ? v.y : bottom; 
        ret.y = (v.y < upper) ? v.y : upper; 

        Debug.Log("Fited position: " + ret);

        return ret;  
    }

    private Vector2 FitAreaPos(Vector2 areaSize, Vector2 nodeSize, Vector2 v)
    {
        var ret = new Vector2(0, 0);

        Debug.Log("Random position: " + v);

        // area origin(0, 0) is center.
        float left = areaSize.x / 2;
        float right =  left - nodeSize.x; // node anchor is upper left ,so right side on the area need much space of node. 
        float upper = areaSize.y / 2;
        float bottom = upper - nodeSize.y; // node anchor is upper left ,so bottom side on the area need much space of node. 

        ret.x = (v.x > -left) ? v.x : -left; 
        ret.x = (ret.x < right) ? ret.x : right; 

        ret.y = (v.y > -bottom) ? v.y : -bottom; 
        ret.y = (ret.y < upper) ? ret.y : upper; 

        return ret;
    }

    private Vector2 RndPos(Vector2 area)
    {
        // area origin(0, 0) is center
        var ratex = Random.value;
        var signx = Random.value < 0.5 ? -1 : 1;
        var ratey = Random.value;
        var signy = Random.value < 0.5 ? -1 : 1;

        return new Vector2(area.x * ratex * signx , area.y * ratey * signy);
    }

    private IEnumerator CreateNode()
    {
        for(int i = 0; i < 10; i++){
            yield return new WaitForSeconds(1);
            var n = Instantiate<GameObject>(nodePrefab, area.transform, false);
            n.GetComponent<CanvasGroup>().alpha = 0;
 
            yield return new WaitForEndOfFrame(); // wait for rect size available.
            var nsize = n.GetComponent<RectTransform>().rect.size;
            n.transform.localPosition = FitAreaPos(areaSize, nsize, RndPos(areaSize));
            n.GetComponent<CanvasGroup>().alpha = 1;
 
            Debug.Log(i + "node pos: " + n.transform.localPosition);
        }
    }
}
