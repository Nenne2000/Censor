using Censor;
using Moq;
using NUnit.Framework;

namespace CensorTest
{
    [TestFixture]
    public class CensorTest
    {
        private static I[] CreateFiniteSequence(string[] messages)
        {
            var seq = new I[messages.Length];
            for (int i = 0; i < messages.Length; i++)
            {
                var a = new Mock<I>();
                a.Setup(x => x.Message).Returns(messages[i]);
                seq[i] = a.Object;
            }
            return seq;
        }

        [TestCase()]
        [TestCase("pippo")]
        [TestCase("pippo","pluto")]
        public void NothingToFilter(params string[] theMsgSequence)
        {
            var seq = CreateFiniteSequence(theMsgSequence);
            var result = CensorClass.Censor(seq, "xyz");
            Assert.That(result, Is.EqualTo(seq));
        }

        [Test]
        public void NullSequence()
        {
            Assert.That(() => CensorClass.Censor(null, "xyz"), Throws.TypeOf<ArgumentNullException>()); 
        }
        
        [Test]
        public void NullElementInSequence()
        {
            var badWord = "xyz";
            var input = CreateFiniteSequence(new[] { "ciao", "ciao ZIOPERA", "ZIOPERA", "auauaeueuaueu" });
            input[0] = null;
            Assert.That(()=> CensorClass.Censor(input, badWord).Any(), Throws.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void OneMatchInTheMiddle()
        {
            var badWord = "ZIOPERA";
            var seq = new Mock<I>();
            seq.Setup(x => x.Message).Returns("ciao io ZIOPERA mi chiamo Mario");
            var result = CensorClass.Censor(new[] { seq.Object }, badWord);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void FilterManyElements()
        {
            var badWord = "ZIOPERA";
            var input = CreateFiniteSequence(new[] { "ciao", "ciao ZIOPERA", "ehi", "ZIOPERA" });
            var expectedOutput = new I[2];
            expectedOutput[0] = input[0];
            expectedOutput[1] = input[2];
            var result = CensorClass.Censor(input, badWord);
            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        /*
         //Con Mock non può funzionare perchè usa tipi reference che sono 
         //diversi ad ogni enumerazione
         [Test]
         public void NothingToFilterInfinite()
         {
             IEnumerable<I> Infinite()
             {
                 while (true)
                 {
                     var a = new Mock<I>();
                     a.Setup(x => x.Message).Returns("puffo");
                     yield return a.Object;
                 }
             }
             var enumerable = Infinite().Take(100);
             var result = CensorClass.Censor(enumerable, "xyz");
             Assert.That(result, Is.EqualTo(enumerable));
         }
         */
    }
}