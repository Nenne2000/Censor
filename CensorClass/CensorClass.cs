//using System.Text.RegularExpressions;

namespace Censor
{

    public interface I
    {
        string Message { get; }
    }

    public static class CensorClass
    {
        public static IEnumerable<I> Censor(IEnumerable<I> sequence, string badWord)
        {
            static IEnumerable<I> Aux_Censor(IEnumerable<I> s, string bw)
            {
                using var iterator = s.GetEnumerator();
                while (iterator.MoveNext())
                {
                    var cur = iterator.Current;
                    if (cur is null) throw new ArgumentNullException(nameof(s), "la sequenza non può contenere elementi nulli");
                    if (!cur.Message.Contains(bw)) yield return cur;
                }
            }
            if(sequence is null) throw new ArgumentNullException(nameof(sequence),"la sequenza non può essere nulla");

            return Aux_Censor(sequence,badWord);
        }
    }
}