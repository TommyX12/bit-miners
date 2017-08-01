using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

static public class Util
{
    
    public const float PI2 = Mathf.PI * 2.0f;
    
    private static Stack<Random.State> _randomSeedStack = new Stack<Random.State>();
    
    static public void PushRandomSeed(int seed)
    {
        _randomSeedStack.Push(Random.state);
        Random.InitState(seed);
    }
    
    static public void PopRandomSeed()
    {
        if (_randomSeedStack.Count == 0) return;
        Random.state = _randomSeedStack.Pop();
    }
    
    static public float[] ColorToFloatArray(Color color)
    {
        return new float[]{color.r, color.g, color.b, color.a};
    }
    
    static public float RotationDist(float start, float end, bool useRadians)
    {
        float cap = useRadians ? PI2 * 2.0f : 360.0f;
        float dif = (end - start) % cap;
        if (dif != dif % (cap / 2)) {
            dif = (dif < 0) ? dif + cap : dif - cap;
        }
        return dif;
    }
    
    static public float GetAngle(Vector2 target, Vector2 origin, bool radian)
    {
        float dx = -(target.x - origin.x);  
        float dy = -(target.y - origin.y);
        return radian ? Mathf.Atan2(dy, dx) : Mathf.Atan2(dy, dx) * 180.0f / Mathf.PI;
    }
    
    static public string AlphaNumericOnly(string str)
    {
        return Regex.Replace(str, "[^A-Za-z0-9_]", "");
    }
    
    static public int StringHash(string str)
    {
        int hash = 5381;
        foreach (char chr in str) hash = ((hash << 5) + hash) + Convert.ToInt32(chr);
        return hash;
    }

    static public Component Make(Component childTemplate)
    {
        Component newObject = (Component)UnityEngine.Object.Instantiate(childTemplate); // Instantiate
        
        return newObject;
    }
    
    static public T Make<T>(Component childTemplate)
        where T:Component
    {
        T newObject = (T)UnityEngine.Object.Instantiate(childTemplate); // Instantiate
        
        return newObject;
    }
    
    static public GameObject Make(GameObject childTemplate)
    {
        GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(childTemplate); // Instantiate
        
        return newObject;
    }
    
    static public Component MakeChild(Transform parentTransform, Component childTemplate)
    {
        Component newObject = (Component)UnityEngine.Object.Instantiate(childTemplate, parentTransform.position, parentTransform.rotation); // Instantiate
        newObject.transform.parent = parentTransform; // Set as child
        newObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        
        return newObject;
    }
    
    static public GameObject MakeChild(Transform parentTransform, GameObject childTemplate)
    {
        GameObject newObject = (GameObject)UnityEngine.Object.Instantiate(childTemplate, parentTransform.position, parentTransform.rotation); // Instantiate
        newObject.transform.parent = parentTransform; // Set as child
        newObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        
        return newObject;
    }
    
    static public T SafeGetComponent<T>(GameObject gameObject)
        where T:Component
    {
        T result;
        if ((result = gameObject.GetComponent<T>()) != null) {
            return result;
        }
        
        return gameObject.AddComponent<T>();
    }
    
    static public T MakeChild<T>(Transform parentTransform, T childTemplate)
        where T:Component
    {
        T newObject = (T)UnityEngine.Object.Instantiate(childTemplate, parentTransform.position, parentTransform.rotation); // Instantiate
        newObject.transform.parent = parentTransform; // Set as child
        newObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        
        return newObject;
    }
    
    static public GameObject MakeEmptyUIContainer(RectTransform parentTransform) {
        GameObject newObject = new GameObject("", typeof(RectTransform));
        newObject.transform.SetParent(parentTransform, false);
        RectTransform newRectTransform = (RectTransform)newObject.transform;
        InitUIRectTransform(newRectTransform);
        
        return newObject;
    }
    
    static public void InitUIRectTransform(RectTransform rectTransform) {
        rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
        rectTransform.offsetMin = new Vector2(0.0f, 0.0f);
        rectTransform.offsetMax = new Vector2(0.0f, 0.0f);
    }
    
    static public void TopLeftUIRectTransform(RectTransform rectTransform) {
        rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
        rectTransform.anchorMax = new Vector2(0.0f, 1.0f);
    }
    
    static public void TopUIRectTransform(RectTransform rectTransform) {
        rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
        rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
    }
    
    static public void DestroyAllChildren(Transform parentTransform)
    {
        foreach(Transform child in parentTransform){
            GameObject.Destroy(child.gameObject);
        }
    }
    
    static public Vector2 CameraToLocal(Camera camera, Vector2 cameraPoint, Transform transform)
    {
        Vector3 globalPoint = cameraPoint;
        globalPoint.z = transform.position.z - camera.transform.position.z;
        return transform.InverseTransformPoint(camera.ScreenToWorldPoint(globalPoint));
    }
    
    static public float Distance(float dx, float dy)
    {
        return (float)Math.Sqrt(dx*dx + dy*dy);
    }
    
    static public float DistanceSq(float dx, float dy)
    {
        return (float)(dx*dx + dy*dy);
    }
    
    static public bool InRange(Vector2 center, Vector2 point, float radius)
    {
        return (point - center).sqrMagnitude < radius * radius;
    }
    
    static public int Mod(int i, int n) {
        return (i % n + n) % n;
    }
    
    static public bool InRange(float val, float lower, float higher) {
        return val >= lower && val <= higher;
    }
    
    static public bool InRange(int val, int lower, int higher) {
        return val >= lower && val <= higher;
    }
    
    static public void SafeInsert<T>(List<T> list, int index, T item) {
        if (index >= list.Count) {
            list.Add(item);
        }
        else {
            list.Insert(index, item);
        }
    }
    
    static public float Clamp(float val, float lower, float higher)
    {
        return val < lower ? lower : (val > higher ? higher : val);
    }
    
    static public int Clamp(int val, int lower, int higher)
    {
        return val < lower ? lower : (val > higher ? higher : val);
    }
    
    static public float Map(float x, float in_min, float in_max, float out_min, float out_max, bool clamped = false)
    {
        float result = (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        if (clamped) return Clamp(result, out_min, out_max);
        return result;
    }
    
    static public int ClosestPow2(int v)
    {
        int p = 2;
        while (p < v) p = p << 1;
        return p;
    }
    
    static public float RandomFloat(float min, float max) 
    {
        return Random.Range(min, max);
    }
    
    static public bool RandomChance(float chance)
    {
        return Random.Range(0.0f, 0.99999999f) < chance;
    }
    
    static public int RandomInt(int min, int max) 
    {
        return Random.Range(min, max);
    }

    static public T GetRandomElement<T>(List<T> list)
    {
        if (list.Count > 0)
        {
            return list[RandomInt(list.Count)];
        }
        else {
            throw new Exception("List is empty");
        }
    }
    
    static public int RandomSelectChance(float[] chanceArray)
    {
        if (chanceArray.Length == 0) return -1;
        
        int options = chanceArray.Length - 1;
        int pointer = 0;
        float total = 0.0f;
        foreach (float i in chanceArray) total += i;
        float rand = RandomFloat(0.0f, total);
        while (pointer < options-1){
            float chance = chanceArray[pointer];
            if (rand < chance) break;
            rand -= chance;
            total -= chance;
            ++pointer;
        }
        return pointer; 
    }
    
    static public T RandomSelectChance<T>(Dictionary<T, float> optionChanceDictionary)
    {
        if (optionChanceDictionary.Count == 0) return default(T);
        
        float total = 0.0f;
        foreach (float i in optionChanceDictionary.Values) total += i;
        float rand = RandomFloat(0.0f, total);
        T result = default(T);
        foreach (T option in optionChanceDictionary.Keys) {
            result = option;
            float chance = optionChanceDictionary[option];
            if (rand < chance) break;
            rand -= chance;
            total -= chance;
        }
        return result; 
    }
    
    static public int RandomInt(int max) 
    {
        return Random.Range(0, max);
    }
    
    static public Vector2 Vec2FromAngle(float angle, float magnitude)
    {
        return new Vector2(
                (float)Math.Cos(angle) * magnitude,
                (float)Math.Sin(angle) * magnitude
                );
    }
    
    static public Color Float4ToColor(float[] floats)
    {
        return new Color(floats[0], floats[1], floats[2], floats[3]);
    }
    
    private static readonly Dictionary<char, int> hexCharToInt = new Dictionary<char, int>() {
        {'0', 0},
        {'1', 1},
        {'2', 2},
        {'3', 3},
        {'4', 4},
        {'5', 5},
        {'6', 6},
        {'7', 7},
        {'8', 8},
        {'9', 9},
        {'a', 10},
        {'b', 11},
        {'c', 12},
        {'d', 13},
        {'e', 14},
        {'f', 15},
    };
    
    static private int HexSubstringToInt(string hex, int i, int j) {
        int result = 0;
        
        for (; i < j; ++i) {
            char c = hex[i];
            result = result * 16 + hexCharToInt[c];
        }
        
        return result;
    }
    
    static public float[] HexToFloat4(string hex) {
        if (hex[0] == '#') hex = hex.Substring(1);
        hex = hex.ToLower();
        int a = 255;
        int r = 255;
        int g = 255;
        int b = 255;
        if (hex.Length == 6) {
            r = HexSubstringToInt(hex, 0, 2);
            g = HexSubstringToInt(hex, 2, 4);
            b = HexSubstringToInt(hex, 4, 6);
        }
        else if (hex.Length == 8) {
            a = HexSubstringToInt(hex, 0, 2);
            r = HexSubstringToInt(hex, 2, 4);
            g = HexSubstringToInt(hex, 4, 6);
            b = HexSubstringToInt(hex, 6, 8);
        }
        else return null;
        
        return new float[]{
            (float)r / 255.0f,
            (float)g / 255.0f,
            (float)b / 255.0f,
            (float)a / 255.0f,
        };
    }
    
    static public Vector3 CameraToWorld(Camera camera, Vector2 screenPos, float z) {
        float relativeDepth = z - camera.transform.position.z;
        return camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, relativeDepth));
    }
    
    static public void SetUIScreenPos(Canvas canvas, Camera camera, Vector2 screenPos, Transform elementTransform)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, camera, out pos);
        elementTransform.position = canvas.transform.TransformPoint(pos);
    }
    
    static public string ColoredRichText(string color, string text) {
        return "<color=" + color + ">" + text + "</color>";
    }

    static public TFilter RaycastAndFilter<TFilter> (Vector2 position) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.zero);

        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.gameObject.GetComponent<TFilter>() != null) {
                Debug.Log(hit.collider.gameObject.name);
                return hit.collider.gameObject.GetComponent<TFilter>();
            }
        }

        return default(TFilter);
    }
    
    static public void PrintCollection<T>(IEnumerable<T> list) {
        string result = "[";
        int i = 0;
        foreach (T item in list) {
            if (i > 0) {
                result += ", ";
            }
            result += item.ToString();
            i++;
        }
        result += "]";
        Debug.Log(result);
    }
    
    static public void ResizeList<T>(List<T> list, int size, T val)
    {
        int cur = list.Count;
        if(size < cur)
            list.RemoveRange(size, cur - size);
        else if(size > cur)
        {
            if(size > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
              list.Capacity = size;
            list.AddRange(Enumerable.Repeat(val, size - cur));
        }
    }
    
    static public float[] GetTextColorBW(float[] color, float threshold = 0.5f) {
        if (Luma(color) < threshold) {
            return new float[]{1, 1, 1, 1};
        }
        else {
            return new float[]{0, 0, 0, 1};
        }
    }
    
    static public string EscapeScriptString(string str) {
        str = str.Replace("\\", "\\\\");
        str = str.Replace("\"", "\\\"");
        str = str.Replace("\'", "\\\'");
        return str;
    }
    
    static public bool Contains<T>(IEnumerable<T> list, T value) {
        foreach (T item in list) {
            if (item.Equals(value)) {
                return true;
            }
        }
        return false;
    }
    
    static public float Luma(float[] color) {
        return Luma(color[0], color[1], color[2]);
    }
    
    static public float Luma(float r, float g, float b) {
        return 0.299f * r + 0.587f * g + 0.114f * b;
    }
    
    static public float Flashing(float t, float min, float max, float speed) {
        return (float)((Math.Cos(t * speed) + 1 / 2) * (max - min) + min);
    }

    // returns true if done moving to target
    // call it constantly while you want to move
    static public bool MoveToTarget(GameObject ToMove, GameObject Target, float speed) {
        Vector3 deltaV = Target.transform.position - ToMove.transform.position;
        if (deltaV.magnitude < speed * Time.fixedDeltaTime)
        {
            ToMove.transform.position = Target.transform.position;
            return true;
        }
        else {
            ToMove.transform.position += deltaV.normalized * speed * Time.fixedDeltaTime;
            return false;
        }
    }

    // i'll make it later :)
    static public bool RotateSingleAxis(GameObject ToRotate, GameObject AxisToRotate, Vector3 AxisToRotateAlong, Vector3 targetRotation, float speed) {
        

        return false;
    }

    static public bool RotateTowards2D(GameObject ToRotate, GameObject Target, float degreespersec) {

        Vector2 dv = Target.transform.position - ToRotate.transform.position;
        float da = Vector2.SignedAngle(ToRotate.transform.up, dv);

        if (Mathf.Abs(da) <= degreespersec * Time.fixedDeltaTime)
        {
            ToRotate.transform.rotation = Quaternion.LookRotation(ToRotate.transform.forward, dv);
            return true;
        }
        else
        {
            ToRotate.transform.Rotate(new Vector3(0, 0, (da / Mathf.Abs(da)) * degreespersec * Time.fixedDeltaTime));
        }

        return false;
    }
}
