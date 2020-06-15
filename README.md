# PosterTool
最近给公司写了一个自定义海报生成工具，主要功能如下


1.数据信息走csv配置读取
表的配置读取解析参考之前博文  https://blog.csdn.net/qq_37310110/article/details/89488295

2.自动化生成节日路径
IO系统的学习及应用参考之前博文 https://blog.csdn.net/qq_37310110/article/details/87880438

3.界面元素的自定义位置拖拽
ui元素拖拽分两种情况，具体如何设置是根据Canvas的模式来说的

第一种：如果Canvas的模式为OverLay

```
public class DragUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private Vector2 offsetPos;  //临时记录点击点与UI的相对位置

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - offsetPos;
    }
     
    public void OnPointerDown(PointerEventData eventData)
    {
        offsetPos = eventData.position - (Vector2)transform.position;
    }

}
```


第二种：如果Canvas的模式为Camera

```
public class DragUI : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.gameObject.GetComponent<RectTransform>(), eventData.position, Camera.main, out position))
        {
            return;
        }
        transform.localPosition = position;
    }

}
```

4.海报背景自助设定
这里用到了一个程序集：System.Windows.Forms，

在unity的安装路径：D:\Package\2018.4.0f1\Unity\Editor\Data\Mono\lib\mono\2.0，导入Plugins文件夹即可

```
 public void SelectMainBg()
    {
        try
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Title = "请选择海报背景";
            od.Multiselect = false;
            od.Filter = "图片文件(*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp";
            if (od.ShowDialog() == DialogResult.OK)
            {
                //Debug.Log(od.FileName);
                StartCoroutine(GetTexture("file://" + od.FileName, mainBg));
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    IEnumerator GetTexture(string url,Image image)
    {
        WWW www = new WWW(url);
        Debug.Log(url);
        yield return www;
        if (www.isDone && www.error == null)
        {
            img = www.texture;
            sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;
            Debug.Log(img.width + " " + img.height);
            byte[] date = img.EncodeToPNG();
        }
        else
        {
            Debug.Log(www.error);
        }
    }
```

 ![img](https://img-blog.csdnimg.cn/20200407162705547.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzM3MzEwMTEw,size_16,color_FFFFFF,t_70)




5.文字颜色大小可通过色板设置
这个功能本来想自己写一个获取色环的工具出来的，后来因为时间上的原因就用了网上现有的色板工具很方便，再二次开发一下加入了字体大小的设置

![img](https://img-blog.csdnimg.cn/2020040716301896.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzM3MzEwMTEw,size_16,color_FFFFFF,t_70)

6.截图保存自定义路径
截图的话 就直接用的unity自带的截图工具

```
 IEnumerator SaveImage(string path)
    {
        ScreenCapture.CaptureScreenshot(path, 5);
        yield return new WaitUntil(() => true);
        //yield return new WaitForSeconds(1);
        Debug.Log(path.Split('/')[5]+"--海报生成成功！！");
    }
```



