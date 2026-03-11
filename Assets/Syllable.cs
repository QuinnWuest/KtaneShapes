namespace Syl
{
    public sealed class Syllable
    {
        public Sound Onset { get; }
        public Sound Nucleus { get; }
        public Sound[] Coda { get; }

        public Syllable(Sound onset, Sound nucleus, Sound[] coda)
        {
            Onset = onset;
            Nucleus = nucleus;
            Coda = coda;
        }

        public override string ToString()
        {
            var onset = Onset.ToString() == "None" ? "" : Onset.ToString();
            var coda = Coda.Join("");
            return $"{onset}{Nucleus}{coda}"; 
        }
    }
}
