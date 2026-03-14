using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Syl;
using System.Collections;
using System.Text.RegularExpressions;

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

    private static readonly Dictionary<Sound, ConsonantInfo> _consonantInfos = new Dictionary<Sound, ConsonantInfo>()
    {
        [Sound.P] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .002f), new Vector3(.002f, .0015f, -.002f), 2, 0),
        [Sound.B] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .002f), new Vector3(.002f, .0015f, -.002f), 2, 180),
        [Sound.K] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .002f), new Vector3(.000f, .0015f, -.003f), 16, 0),
        [Sound.G] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .003f), new Vector3(.000f, .0015f, -.002f), 16, 180),
        [Sound.T] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .002f), new Vector3(.002f, .0015f, -.002f), 3, 0),
        [Sound.D] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .002f), new Vector3(.002f, .0015f, -.002f), 4, 0),
        [Sound.TH] = new ConsonantInfo(new Vector3(-0.00325f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .003f), new Vector3(-.002f, .0015f, -.003f), 19, 0),
        [Sound.DH] = new ConsonantInfo(new Vector3(0.00325f, .0015f, 0.000f), new Vector3(.002f, .0015f, .003f), new Vector3(.002f, .0015f, -.003f), 19, 180),
        [Sound.SH] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.003f, .0015f, -.0015f), new Vector3(.003f, .0015f, -.0015f), 5, 0),
        [Sound.ZH] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.003f, .0015f, .0025f), new Vector3(.003f, .0015f, .0025f), 6, 0),
        [Sound.CH] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0035f, .0015f, -.0015f), new Vector3(.0035f, .0015f, -.0015f), 7, 0),
        [Sound.J] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 8, 0),
        [Sound.F] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(.0025f, .0015f, .0025f), new Vector3(-.0025f, .0015f, -.0025f), 9, 0),
        [Sound.V] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0025f, .0015f, .0025f), new Vector3(.0025f, .0015f, -.0025f), 9, 90),
        [Sound.S] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(.0025f, .0015f, .0025f), new Vector3(-.0025f, .0015f, -.0025f), 24, 90),
        [Sound.Z] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0025f, .0015f, .0025f), new Vector3(.0025f, .0015f, -.0025f), 24, 0),
        [Sound.H] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0025f, .0015f, .001f), new Vector3(.0025f, .0015f, .001f), 20, 0),
        [Sound.M] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 11, 0),
        [Sound.N] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 12, 0),
        [Sound.NG] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 13, 0),
        [Sound.L] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0025f, .0015f, .000f), new Vector3(.0025f, .0015f, -.000f), 15, 270),
        [Sound.R] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.0025f, .0015f, .000f), new Vector3(.0025f, .0015f, -.000f), 15, 90),
        [Sound.W] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 15, 0),
        [Sound.Y] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.000f, .0015f, .0025f), new Vector3(.000f, .0015f, -.0025f), 15, 180),
        [Sound.None] = new ConsonantInfo(new Vector3(0.000f, .0015f, 0.000f), new Vector3(-.002f, .0015f, .002f), new Vector3(.002f, .0015f, -.002f), 14, 45),
    };
    private static readonly Dictionary<Sound, Color32> _vowelInfos = new Dictionary<Sound, Color32>()
    {
        [Sound.I] = new Color32(220, 20, 60, 255),    // ɪ      bIg
        [Sound.II] = new Color32(77, 166, 255, 255),  // iː     tEAch
        [Sound.E] = new Color32(255, 215, 0, 255),    // ɛ      pEt
        [Sound.A] = new Color32(255, 140, 0, 255),    // æ      cAt
        [Sound.AA] = new Color32(139, 0, 0, 255),     // ɑː     fAther
        [Sound.U] = new Color32(34, 139, 34, 255),    // ʌ      bUtter
        [Sound.OO] = new Color32(60, 60, 240, 255),   // ʊ      bOOk
        [Sound.UU] = new Color32(0, 128, 128, 255),   // uː     fOOd
        [Sound.AI] = new Color32(255, 0, 255, 255),   // aɪ     lIme
        [Sound.AU] = new Color32(80, 235, 215, 255),  // aʊ     clOWn
        [Sound.OI] = new Color32(138, 43, 226, 255),  // ɔɪ     nOIse
        [Sound.OU] = new Color32(200, 170, 130, 255), // oʊ     bOAt
        [Sound.EI] = new Color32(50, 205, 50, 255),   // eɪ     clAIm
        [Sound.Ə] = new Color32(255, 137, 212, 255),  // ə      Among
    };

    private static readonly Dictionary<Sound, string[]> _infoForLogging = new Dictionary<Sound, string[]>()
    {
        [Sound.P] = new[] { "up-pointing pentagon", "an up-pointing pentagon" },
        [Sound.B] = new[] { "down-pointing pentagon", "a down-pointing pentagon" },
        [Sound.K] = new[] { "upright kite", "an upright kite" },
        [Sound.G] = new[] { "upside-down kite", "an upside-down kite" },
        [Sound.T] = new[] { "hexagon", "a hexagon" },
        [Sound.D] = new[] { "octagon", "an octagon" },
        [Sound.TH] = new[] { "Pac-Man facing right", "a Pac-Man facing right" },
        [Sound.DH] = new[] { "Pac-Man facing left", "a Pac-Man facing left" },
        [Sound.SH] = new[] { "spade", "a spade" },
        [Sound.ZH] = new[] { "heart", "a heart" },
        [Sound.CH] = new[] { "club", "a club" },
        [Sound.J] = new[] { "diamond", "a diamond" },
        [Sound.F] = new[] { "top-right/bottom-left circle pair", "a top-right/bottom-left circle pair" },
        [Sound.V] = new[] { "top-left/bottom-right circle pair", "a top-left/bottom-right circle pair" },
        [Sound.S] = new[] { "top-right/bottom-left square pair", "a top-right/bottom-left square pair" },
        [Sound.Z] = new[] { "top-left/bottom-right square pair", "a top-left/bottom-right square pair" },
        [Sound.H] = new[] { "speech bubble", "a speech bubble" },
        [Sound.M] = new[] { "4-pointed star", "a 4-pointed star" },
        [Sound.N] = new[] { "5-pointed star", "a 5-pointed star" },
        [Sound.NG] = new[] { "6-pointed star", "a 6-pointed star" },
        [Sound.L] = new[] { "left arrow", "a left arrow" },
        [Sound.R] = new[] { "right arrow", "a right arrow" },
        [Sound.W] = new[] { "up arrow", "an up arrow" },
        [Sound.Y] = new[] { "down arrow", "a down arrow" },
        [Sound.None] = new[] { "X", "an X" },
        [Sound.I] = new[] { "a red" },
        [Sound.II] = new[] { "a sky blue" },
        [Sound.E] = new[] { "a yellow" },
        [Sound.A] = new[] { "an orange" },
        [Sound.AA] = new[] { "a maroon" },
        [Sound.U] = new[] { "a green" },
        [Sound.OO] = new[] { "a blue" },
        [Sound.UU] = new[] { "a teal" },
        [Sound.Ə] = new[] { "a pink" },
        [Sound.AI] = new[] { "a magenta" },
        [Sound.AU] = new[] { "a cyan" },
        [Sound.OI] = new[] { "a purple" },
        [Sound.OU] = new[] { "a tan" },
        [Sound.EI] = new[] { "a lime" },
    };
    private static readonly string[] _ordinals = new string[] { "first", "second", "third", "fourth" };

    private KeyValuePair<string, Syllable[]> _chosenWord;
    private string _strInput = "";
    private List<int> _input = new List<int>();
    private Coroutine _timer;
    private Coroutine _textAnimation;
    private int _buttonCount;
    private bool _submissionTimerActivated;
    private bool _isAnimationRunning;

    private void Awake()
    {
        _moduleId = _moduleIdCounter++;
        _chosenWord = Data._wordList.PickRandom();

        _buttonCount = _chosenWord.Value.Length;
        var syllables = _chosenWord.Value;
        SetButtons(syllables);
        Debug.LogFormat("[Shapes #{0}] Chosen word: {1}", _moduleId, _chosenWord.Key);
        Debug.LogFormat("[Shapes #{0}] Syllables: {1}", _moduleId, _chosenWord.Value.Join(" "));

        for (int i = 0; i < syllables.Length; i++)
        {
            Debug.Log($"[Shapes #{_moduleId}] The {_ordinals[i]} button is {_infoForLogging[syllables[i].Nucleus][0]} {_infoForLogging[syllables[i].Onset][0]}{(syllables[i].Coda.Length == 0 ? "" : $" with {syllables[i].Coda.Select(x => _infoForLogging[x][1]).Join(" and ")} on it")}.");
        }

        LetterTM.text = "";
        for (int i = 0; i < _buttonCount; i++)
            ButtonSels[i].OnInteract += ButtonPress(i);
    }

    private void SetButtons(Syllable[] syllableInfo)
    {
        ModuleSelectable.Children = Enumerable.Range(0, _buttonCount).Select(i => ButtonSels[i]).ToArray();
        ModuleSelectable.UpdateChildrenProperly();
        for (int i = 3; i >= 0; i--)
            if (i >= _buttonCount)
                ButtonObjs[i].SetActive(false);

        if (_buttonCount == 2)
            for (int ix = 0; ix < 2; ix++)
            {
                ButtonObjs[ix].transform.localPosition = new Vector3(ix * 0.06f - 0.03f, 0.02f, ix * -0.06f + 0.03f);
                ButtonObjs[ix].transform.localScale = new Vector3(4, 4, 4);
            }
        else if (_buttonCount == 3)
            for (int ix = 0; ix < 3; ix++)
            {
                ButtonObjs[ix].transform.localPosition = new Vector3((ix == 0) ? 0f : (ix - 1.5f) * 0.08f, 0.02f, (ix > 0 ? -1f : 1f) * 0.035f);
                ButtonObjs[ix].transform.localScale = new Vector3(4, 4, 4);
            }
        else if (_buttonCount == 4)
            for (int ix = 0; ix < 4; ix++)
            {
                ButtonObjs[ix].transform.localPosition = new Vector3((ix == 0 || ix == 3) ? 0f : (ix == 1 ? -0.045f : 0.045f), 0.02f, (ix == 0) ? 0.045f : (ix == 3 ? -0.045f : 0f));
                ButtonObjs[ix].transform.localScale = new Vector3(3, 4, 3);
            }

        for (int i = 0; i < syllableInfo.Length; i++)
        {
            var onset = syllableInfo[i].Onset;
            ButtonObjs[i].transform.GetChild(1).GetComponent<MeshFilter>().mesh = ModelMeshes[_consonantInfos[onset].MeshIx];
            ButtonObjs[i].transform.GetChild(1).localEulerAngles = new Vector3(90f, _consonantInfos[onset].Rotation, 0f);
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
                ButtonObjs[i].transform.GetChild(2).localEulerAngles = new Vector3(90f, _consonantInfos[coda[0]].Rotation, 0f);
                ButtonObjs[i].transform.GetChild(2).localPosition = new Vector3(_consonantInfos[onset].ExtrusionMainInfo.x, _consonantInfos[onset].ExtrusionMainInfo.y, _consonantInfos[onset].ExtrusionMainInfo.z);
                ButtonObjs[i].transform.GetChild(3).gameObject.SetActive(false);
                ButtonObjs[i].transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (coda.Length == 2)
            {
                ButtonObjs[i].transform.GetChild(2).gameObject.SetActive(false);
                ButtonObjs[i].transform.GetChild(3).GetComponent<MeshFilter>().mesh = ModelMeshes[_consonantInfos[coda[0]].MeshIx];
                ButtonObjs[i].transform.GetChild(4).GetComponent<MeshFilter>().mesh = ModelMeshes[_consonantInfos[coda[1]].MeshIx];
                ButtonObjs[i].transform.GetChild(3).localEulerAngles = new Vector3(90f, _consonantInfos[coda[0]].Rotation, 0f);
                ButtonObjs[i].transform.GetChild(4).localEulerAngles = new Vector3(90f, _consonantInfos[coda[1]].Rotation, 0f);
                ButtonObjs[i].transform.GetChild(3).localPosition = new Vector3(_consonantInfos[onset].ExtrusionAInfo.x, _consonantInfos[onset].ExtrusionAInfo.y, _consonantInfos[onset].ExtrusionAInfo.z);
                ButtonObjs[i].transform.GetChild(4).localPosition = new Vector3(_consonantInfos[onset].ExtrusionBInfo.x, _consonantInfos[onset].ExtrusionBInfo.y, _consonantInfos[onset].ExtrusionBInfo.z);
            }
            Color color = _vowelInfos[syllableInfo[i].Nucleus];
            ButtonObjs[i].transform.GetChild(1).GetComponent<MeshRenderer>().material.color = color;
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
        yield return new WaitForSeconds(0.75f);
        _submissionTimerActivated = true;
    }

    private void Update()
    {
        if (!_submissionTimerActivated)
            return;
        CheckInput(_input);
        _input = new List<int>();
        _submissionTimerActivated = false;
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
            Debug.LogFormat("[Shapes #{0}] Inputted: {1} -> {2}. Expected {3}. Strike.", _moduleId, input.Select(i => i + 1).Join(""), ch, _chosenWord.Key[_strInput.Length]);
            _strInput = "";
            StartCoroutine(StrikeTimer());
        }
        else
        {
            Debug.LogFormat("[Shapes #{0}] Inputted: {1} -> {2}. Correct.", _moduleId, input.Select(i => i + 1).Join(""), ch);
            _strInput += ch;
        }
        bool solving = _strInput == _chosenWord.Key;
        if (_textAnimation != null)
            StopCoroutine(_textAnimation);
        _textAnimation = StartCoroutine(AnimateLetter(ch, correct));
        if (solving)
            StartCoroutine(SolveTimer());
    }

    private IEnumerator StrikeTimer()
    {
        yield return new WaitForSeconds(1f);
        Module.HandleStrike();
    }

    private IEnumerator SolveTimer()
    {
        yield return new WaitForSeconds(1f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
        _moduleSolved = true;
        Module.HandlePass();
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
        return result;
    }

    private IEnumerator AnimateLetter(char letter, bool correct)
    {
        _isAnimationRunning = true;
        var red = new Color32(255, 0, 0, 255);
        var green = new Color32(0, 255, 0, 255);
        var color = correct ? green : red;
        LetterTM.text = letter.ToString();
        LetterTM.color = color;
        var duration = 1f;
        var holdTime = 0.8f;
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
        _isAnimationRunning = false;
    }

#pragma warning disable 0414
    private readonly string TwitchHelpMessage = "!{0} press 1 2 3, 1 3 2, 3 2 1 [Press the buttons (numbered in reading order). Semicolons or commas separate input sequences.]";
#pragma warning restore 0414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = Regex.Replace(command.Trim().ToLowerInvariant(), @"^\s+", " ");
        if (Regex.Match(command, @"^\s*reset\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant).Success)
        {
            _input = new List<int>();
            _strInput = "";
            Debug.LogFormat("[Shapes #{0}] Input has been reset via Twitch Plays command.", _moduleId);
            yield return "sendtochat Input has been reset.";
            yield break;
        }
        if (command.StartsWith("press "))
            command = command.Substring(6);
        else if (command.StartsWith("submit "))
            command = command.Substring(7);
        var cmds = command.Split(new[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        var list = new List<int>();
        foreach (var cmd in cmds)
        {
            foreach (var ch in cmd)
            {
                if (ch == ' ')
                    continue;
                var num = ch - '0' - 1;
                if (num < 0 || num >= _buttonCount)
                {
                    yield return $"sendtochaterror {ch} is not a valid button. Command ignored.";
                    yield break;
                }
                list.Add(num);
            }
            list.Add(-1);
        }
        yield return null;
        yield return "solve";
        yield return "strike";
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == -1)
            {
                yield return new WaitForSeconds(0.1f);
                if (_timer != null)
                    StopCoroutine(_timer);
                _submissionTimerActivated = true;
                yield return null;
                while (_isAnimationRunning)
                    yield return null;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                ButtonSels[list[i]].OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
        if (_input.Count > 0)
        {
            if (_timer != null)
                StopCoroutine(_timer);
            _submissionTimerActivated = true;
            yield return null;
            while (_isAnimationRunning && !_moduleSolved)
                yield return null;
        }
        while (!_moduleSolved)
        {
            int progress = _strInput.Length;
            if (progress == _chosenWord.Key.Length)
                yield break;

            char ch = _chosenWord.Key[progress];
            yield return StartCoroutine(SubmitSequence(EncodeLetter(ch)));
        }
    }

    private IEnumerator SubmitSequence(IEnumerable<int> presses)
    {
        foreach (int ix in presses)
        {
            ButtonSels[ix].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        if (_timer != null)
            StopCoroutine(_timer);
        _submissionTimerActivated = true;
        yield return null;
        while (_isAnimationRunning && !_moduleSolved)
            yield return null;
        yield return new WaitForSeconds(0.1f);
    }

    private static readonly Dictionary<char, string> _morseEncode = new Dictionary<char, string>()
    {
        ['A'] = ".-",
        ['B'] = "-...",
        ['C'] = "-.-.",
        ['D'] = "-..",
        ['E'] = ".",
        ['F'] = "..-.",
        ['G'] = "--.",
        ['H'] = "....",
        ['I'] = "..",
        ['J'] = ".---",
        ['K'] = "-.-",
        ['L'] = ".-..",
        ['M'] = "--",
        ['N'] = "-.",
        ['O'] = "---",
        ['P'] = ".--.",
        ['Q'] = "--.-",
        ['R'] = ".-.",
        ['S'] = "...",
        ['T'] = "-",
        ['U'] = "..-",
        ['V'] = "...-",
        ['W'] = ".--",
        ['X'] = "-..-",
        ['Y'] = "-.--",
        ['Z'] = "--.."
    };

    private IEnumerable<int> EncodeLetter(char ch)
    {
        ch = char.ToUpperInvariant(ch);
        if (_buttonCount == 2)
        {
            foreach (char c in _morseEncode[ch])
                yield return c == '.' ? 0 : 1;
            yield break;
        }
        if (_buttonCount == 3)
        {
            int value = ch - 'A';
            yield return value / 9;
            yield return (value / 3) % 3;
            yield return value % 3;
            yield break;
        }
        if (_buttonCount == 4)
        {
            string[] grid =
            {
                "..A..",
                "BCDEF",
                "GHIJK",
                "LM?NO",
                "PQRST",
                "UVWXY",
                "..Z.."
            };
            int targetRow = -1, targetCol = -1;
            for (int r = 0; r < grid.Length; r++)
                for (int c = 0; c < grid[r].Length; c++)
                    if (grid[r][c] == ch)
                    {
                        targetRow = r;
                        targetCol = c;
                    }
            int row = 3, col = 2;
            while (row > targetRow)
            {
                yield return 0;
                row--;
            }
            while (row < targetRow)
            {
                yield return 3;
                row++;
            }
            while (col > targetCol)
            {
                yield return 1;
                col--;
            }
            while (col < targetCol)
            {
                yield return 2;
                col++;
            }
        }
    }
}
