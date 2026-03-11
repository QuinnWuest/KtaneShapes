using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Syl;
using System;
using System.Collections;

public class ShapesScript : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo BombInfo;
    public KMAudio Audio;

    public Mesh[] ModelMeshes;
    public KMSelectable[] ButtonSels;
    public GameObject[] ButtonObjs;
    public KMSelectable ModuleSelectable;

    public TextMesh LetterTM;

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private bool _moduleSolved;


    private readonly Dictionary<Sound, ConsonantInfo> _consonantInfos = new Dictionary<Sound, ConsonantInfo>()
    {
        [Sound.None] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .002f), new Vector3(.002f, .0015f, -.002f), 14, 45),
        [Sound.B] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .002f), new Vector3(.002f, .0015f, -.002f), 2, 0),
        [Sound.P] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .002f), new Vector3(.002f, .0015f, -.002f), 2, 180),
        [Sound.G] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .002f), new Vector3(.000f, .0015f, -.003f), 16, 0),
        [Sound.K] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .003f), new Vector3(.000f, .0015f, -.002f), 16, 180),
        [Sound.D] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .002f), new Vector3(.002f, .0015f, -.002f), 3, 0),
        [Sound.T] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .002f), new Vector3(.002f, .0015f, -.002f), 4, 0),
        [Sound.F] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0025f, .0015f, .0025f), new Vector3(.0025f, .0015f, -.0025f), 9, 0),
        [Sound.V] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(.0025f, .0015f, .0025f), new Vector3(-.0025f, .0015f, -.0025f), 9, 90),
        [Sound.S] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(.0025f, .0015f, .0025f), new Vector3(-.0025f, .0015f, -.0025f), 24, 0),
        [Sound.Z] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0025f, .0015f, .0025f), new Vector3(.0025f, .0015f, -.0025f), 24, 90),
        [Sound.SH] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.003f, .0015f, -.0015f), new Vector3(.003f, .0015f, -.0015f), 5, 0),
        [Sound.ZH] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.003f, .0015f, .0025f), new Vector3(.003f, .0015f, .0025f), 6, 0),
        [Sound.TH] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0035f, .0015f, -.0015f), new Vector3(.0035f, .0015f, -.0015f), 7, 0),
        [Sound.DH] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 8, 0),
        [Sound.CH] = new ConsonantInfo(new Vector3(0.0025f, .0015f, 0.000f), new Vector3(.002f, .0015f, .003f), new Vector3(.002f, .0015f, -.003f), 19, 0),
        [Sound.J] = new ConsonantInfo(new Vector3(-0.0025f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .003f), new Vector3(-.002f, .0015f, -.003f), 19, 180),
        [Sound.H] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(.000f, .0015f, .003f), new Vector3(.000f, .0015f, -.003f), 17, 0),
        [Sound.M] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 11, 0),
        [Sound.N] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0025f, .0015f, .001f), new Vector3(.0025f, .0015f, .001f), 12, 0),
        [Sound.NG] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 13, 0),
        [Sound.L] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0025f, .0015f, .000f), new Vector3(.0025f, .0015f, -.000f), 15, 270),
        [Sound.R] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0025f, .0015f, .000f), new Vector3(.0025f, .0015f, -.000f), 15, 90),
        [Sound.W] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 15, 0),
        [Sound.Y] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 15, 180)
    };
    private readonly Dictionary<Sound, Color32> _vowelInfos = new Dictionary<Sound, Color32>()
    {
        [Sound.I] = new Color32(220, 20, 60, 255),
        [Sound.II] = new Color32(77, 166, 255, 255),
        [Sound.E] = new Color32(255, 215, 0, 255),
        [Sound.A] = new Color32(255, 140, 0, 255),
        [Sound.AA] = new Color32(139, 0, 0, 255),
        [Sound.O] = new Color32(34, 139, 34, 255),
        [Sound.U] = new Color32(75, 0, 130, 255),
        [Sound.OO] = new Color32(0, 128, 128, 255),
        [Sound.Ə] = new Color32(255, 105, 180, 255),
        [Sound.AI] = new Color32(255, 0, 255, 255),
        [Sound.AU] = new Color32(255, 191, 0, 255),
        [Sound.OI] = new Color32(138, 43, 226, 255),
        [Sound.OU] = new Color32(139, 69, 19, 255),
        [Sound.EI] = new Color32(50, 205, 50, 255)
    };

    KeyValuePair<string, Syllable[]> _chosenWord;

    private string _strInput = "";
    private List<int> _input = new List<int>();
    private Coroutine _timer;
    private int _buttonCount;

    private void Awake()
    {
        _moduleId = _moduleIdCounter++;

        _chosenWord = Data._wordList.PickRandom();

        var tempStr = "TEAMMATE";
        var x = new KeyValuePair<string, Syllable[]>(tempStr, Data._wordList[tempStr]);
        _chosenWord = x;

        SetButtons(_chosenWord.Value);
        _buttonCount = _chosenWord.Value.Length;
        Debug.LogFormat("[Module Name #{0}] Chosen word: {1}", _moduleId, _chosenWord.Key);
        Debug.LogFormat("[Module Name #{0}] Syllables: {1}", _moduleId, _chosenWord.Value.Join(" "));

        LetterTM.text = "";

        for (int i = 0; i < _buttonCount; i++)
            ButtonSels[i].OnInteract += ButtonPress(i);
    }

    private void SetButtons(Syllable[] syllableInfo)
    {
        int buttonCount = syllableInfo.Length;
        var children = Enumerable.Range(0, buttonCount).Select(i => ButtonSels[i]).ToArray();
        ModuleSelectable.Children = children;
        ModuleSelectable.UpdateChildrenProperly();
        for (int i = 3; i >= 0; i--)
            if (i >= buttonCount)
                ButtonObjs[i].SetActive(false);

        if (buttonCount == 2)
            for (int ix = 0; ix < 2; ix++)
            {
                ButtonObjs[ix].transform.localPosition = new Vector3(ix * 0.06f - 0.03f, 0.02f, ix * -0.06f + 0.03f);
                ButtonObjs[ix].transform.localScale = new Vector3(4, 4, 4);
            }
        else if (buttonCount == 3)
            for (int ix = 0; ix < 3; ix++)
            {
                ButtonObjs[ix].transform.localPosition = new Vector3((ix == 0) ? 0f : (ix - 1.5f) * 0.08f, 0.02f, (ix > 0 ? -1f : 1f) * 0.035f);
                ButtonObjs[ix].transform.localScale = new Vector3(4, 4, 4);
            }
        else if (buttonCount == 4)
            for (int ix = 0; ix < 4; ix++)
            {
                ButtonObjs[ix].transform.localPosition = new Vector3((ix == 0 || ix == 3) ? 0f : (ix == 1 ? -0.045f : 0.045f), 0.02f, (ix == 0) ? 0.045f : (ix == 3 ? -0.045f : 0f));
                ButtonObjs[ix].transform.localScale = new Vector3(3, 4, 3);
            }

        for (int i = 0; i < syllableInfo.Length; i++)
        {
            var onset = syllableInfo[i].Onset;
            ButtonObjs[i].transform.GetChild(0).GetComponent<MeshFilter>().mesh = ModelMeshes[_consonantInfos[onset].MeshIx];
            ButtonObjs[i].transform.GetChild(0).localEulerAngles = new Vector3(90f, _consonantInfos[onset].Rotation, 0f);
            var coda = syllableInfo[i].Coda;
            if (coda.Length == 0)
            {
                ButtonObjs[i].transform.GetChild(2).gameObject.SetActive(false);
                ButtonObjs[i].transform.GetChild(3).gameObject.SetActive(false);
                ButtonObjs[i].transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (coda.Length == 1)
            {
                ButtonObjs[i].transform.GetChild(2).GetComponent<MeshFilter>().mesh = ModelMeshes[_consonantInfos[coda[0]].MeshIx];
                ButtonObjs[i].transform.GetChild(2).localPosition = new Vector3(_consonantInfos[onset].ExtrusionMainInfo.x, _consonantInfos[onset].ExtrusionMainInfo.y, _consonantInfos[onset].ExtrusionMainInfo.z);
                ButtonObjs[i].transform.GetChild(3).gameObject.SetActive(false);
                ButtonObjs[i].transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (coda.Length == 2)
            {
                ButtonObjs[i].transform.GetChild(2).gameObject.SetActive(false);
                ButtonObjs[i].transform.GetChild(3).GetComponent<MeshFilter>().mesh = ModelMeshes[_consonantInfos[coda[0]].MeshIx];
                ButtonObjs[i].transform.GetChild(4).GetComponent<MeshFilter>().mesh = ModelMeshes[_consonantInfos[coda[1]].MeshIx];
                ButtonObjs[i].transform.GetChild(3).localPosition = new Vector3(_consonantInfos[onset].ExtrusionAInfo.x, _consonantInfos[onset].ExtrusionAInfo.y, _consonantInfos[onset].ExtrusionAInfo.z);
                ButtonObjs[i].transform.GetChild(4).localPosition = new Vector3(_consonantInfos[onset].ExtrusionBInfo.x, _consonantInfos[onset].ExtrusionBInfo.y, _consonantInfos[onset].ExtrusionBInfo.z);
            }
            var nucleus = syllableInfo[i].Nucleus;
            Color color = _vowelInfos[nucleus];
            ButtonObjs[i].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = color;
            for (int j = 2; j <= 4; j++)
                ButtonObjs[i].transform.GetChild(j).GetComponent<MeshRenderer>().material.color = color * 0.6f;
        }
    }

    private KMSelectable.OnInteractHandler ButtonPress(int i)
    {
        return delegate ()
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
            ButtonSels[i].AddInteractionPunch(0.5f);
            if (_moduleSolved)
                return false;
            if (_timer != null)
                StopCoroutine(_timer);
            _timer = StartCoroutine(Timer());
            _input.Add(i);
            return false;
        };
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1.5f);
        CheckInput(_input);
        _input = new List<int>();
    }

    private void CheckInput(List<int> input)
    {
        char ch = '?';
        if (_buttonCount == 2)
            ch = DecodeTwoButton(input);
        else if (_buttonCount == 3)
            ch = DecodeThreeButton(input);
        else if (_buttonCount == 4)
            ch = DecodeFourButton(input);

        bool correct = _chosenWord.Key[_strInput.Length] == ch;
        if (!correct)
        {
            Debug.LogFormat("[Module Name #{0}] Inputted: {1} -> {2}. Expected {3}. Strike.", _moduleId, input.Join(""), ch, _chosenWord.Key[_strInput.Length]);
            _strInput = "";
        }
        else
        {
            Debug.LogFormat("[Module Name #{0}] Inputted: {1} -> {2}. Correct.", _moduleId, input.Join(""), ch);
            _strInput += ch;
        }
        bool solving = _strInput == _chosenWord.Key;
        StartCoroutine(AnimateLetter(ch, correct, solving));
    }

    private char DecodeTwoButton(List<int> input)
    {
        if (input == null || input.Count == 0)
            return '?';

        string morse = input.Select(x => x == 0 ? '.' : '-').Join("");

        var morseTable = new Dictionary<string, char>()
        {
            {".-", 'A'},
            {"-...", 'B'},
            {"-.-.", 'C'},
            {"-..", 'D'},
            {".", 'E'},
            {"..-.", 'F'},
            {"--.", 'G'},
            {"....", 'H'},
            {"..", 'I'},
            {".---", 'J'},
            {"-.-", 'K'},
            {".-..", 'L'},
            {"--", 'M'},
            {"-.", 'N'},
            {"---", 'O'},
            {".--.", 'P'},
            {"--.-", 'Q'},
            {".-.", 'R'},
            {"...", 'S'},
            {"-", 'T'},
            {"..-", 'U'},
            {"...-", 'V'},
            {".--", 'W'},
            {"-..-", 'X'},
            {"-.--", 'Y'},
            {"--..", 'Z'}
        };
        return morseTable.ContainsKey(morse) ? morseTable[morse] : '?';
    }
    private char DecodeThreeButton(List<int> input)
    {
        if (input == null || input.Count != 3)
            return '?';

        if (input.Any(x => x < 0 || x > 2))
            return '?';

        int value = input[0] * 9 + input[1] * 3 + input[2];

        return value < 26 ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[value] : '?';
    }

    private char DecodeFourButton(List<int> input)
    {
        if (input == null)
            return '?';

        string[] grid = new string[]
        {
            "..A..",
            "BCDEF",
            "GHIJK",
            "LM?NO",
            "PQRST",
            "UVWXY",
            "..Z.."
        };

        int row = 3;
        int col = 2;

        foreach (int press in input)
        {
            if (press == 0)
                row--;
            else if (press == 1)
                col--;
            else if (press == 2)
                col++;
            else if (press == 3)
                row++;
            if (row < 0 || row >= grid.Length || col < 0 || col >= grid[row].Length)
                return '?';
            if (grid[row][col] == '.')
                return '?';
        }
        char result = grid[row][col];
        return result == '?' ? '?' : result;
    }

    private IEnumerator AnimateLetter(char letter, bool correct, bool solving)
    {
        yield return null;
        var red = new Color32(255, 0, 0, 255);
        var green = new Color32(0, 255, 0, 255);
        var color = correct ? green : red;
        LetterTM.text = letter.ToString();
        LetterTM.color = color;
        var duration = 1.5f;
        var holdTime = 1f;
        var fadeTime = duration - holdTime;
        var elapsed = 0f;
        Audio.PlaySoundAtTransform("nyoom", transform);
        while (elapsed < duration)
        {
            LetterTM.transform.localScale = new Vector3(Easing.OutQuad(elapsed, 0, 0.01f, duration), Easing.OutQuad(elapsed, 0, 0.01f, duration), 1000f);
            LetterTM.color = new Color32(color.r, color.g, color.b, (byte)(elapsed < holdTime ? 255 : Easing.OutQuad(elapsed - holdTime, 255, 0, duration - holdTime)));
            yield return null;
            elapsed += Time.deltaTime;
        }
        LetterTM.transform.localScale = new Vector3(0.01f, 0.01f, 1000f);
        LetterTM.text = "";
        if (!correct)
        {
            Module.HandleStrike();
        }
        if (solving)
        {
            _moduleSolved = true;
            Module.HandlePass();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
        }
    }
}
