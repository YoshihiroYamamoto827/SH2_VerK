using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueManager : MonoBehaviour
{
    private Text xaxisvalue, yaxisvalue;
    private int xvalue, yvalue;
    private Dropdown ddtmp;
    private string objectname;

    // Start is called before the first frame update
    void Start()
    {
        //�e�L�X�g��GameObject��T���A���ꂼ��̍��W��\���ϐ���0�ŏ�����
        xaxisvalue = GameObject.Find("XAxisValue").GetComponent<Text>();
        yaxisvalue = GameObject.Find("YAxisValue").GetComponent<Text>();
        xvalue = 0;
        yvalue = 0;

        //X���W�AY���W���ꂼ��ɂ��ď���������0��\��
        xaxisvalue.text = xvalue.ToString();
        yaxisvalue.text = yvalue.ToString();

        ddtmp = GameObject.Find("ObjectChooseDropdown").GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void XAxisAdd()
    {
        if(0 <= xvalue && xvalue < 99)
        {
            xvalue++;
            xaxisvalue.text = xvalue.ToString();
        }
    }

    public void XAxisSub()
    {
        if (0 < xvalue && xvalue <= 99)
        {
            xvalue--;
            xaxisvalue.text = xvalue.ToString();
        }
    }

    public void YAxisAdd()
    {
        if (0 <= xvalue && xvalue < 99)
        {
            yvalue++;
            yaxisvalue.text = yvalue.ToString();
        }
    }

    public void YAxisSub()
    {
        if (0 < xvalue && xvalue <= 99)
        {
            yvalue--;
            yaxisvalue.text = yvalue.ToString();
        }
    }

    public int Sendxvalue()
    {
        return xvalue;
    }

    public int Sendyvalue()
    {
        return yvalue;
    }

    public string Sendobjectname()
    {
        objectname = ddtmp.options[ddtmp.value].text;
        return objectname;
    }
}
