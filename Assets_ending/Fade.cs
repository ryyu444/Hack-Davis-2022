using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public GameObject square;

    // Start is called before the first frame update
    
    // Update is called once per frame
    public void Start()
    {
        StartCoroutine(Fadetoblack(false));
        StartCoroutine(Fadetoblack());
    }

    public IEnumerator Fadetoblack(bool fading = true, int speed = 1)
    {
        Color objcolor = square.GetComponent<Image>().color;
        float amount;

        

        if (fading)
        {
            while (square.GetComponent<Image>().color.a < 1.1)
            {
                yield return new WaitForSeconds(28);
                amount = objcolor.a + (speed/100);

                objcolor = new Color(objcolor.r, objcolor.g, objcolor.b, amount);
                square.GetComponent<Image>().color = objcolor;
                yield return null;
            }
        } else
        {
            while (square.GetComponent<Image>().color.a > 0)
            {
                amount = objcolor.a - (speed * Time.deltaTime);

                objcolor = new Color(objcolor.r, objcolor.g, objcolor.b, amount);
                square.GetComponent<Image>().color = objcolor;
                yield return null;
            }
        }
        
    }
}
