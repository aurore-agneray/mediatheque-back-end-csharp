namespace Infrastructure.BnfApi.Tests.BnfApiStaticManager
{
    [TestClass]
    public class GetSimpleSearchConditions
    {
        /// <summary>
        /// The notices quantity is included into the allowed list : 20, 100, 200, 500, 1000
        /// </summary>
        [TestMethod]
        public void WithARelevantNoticesQuantity()
        {
            string criterion = "pokemon";

            foreach (var noticesQty in BnfGlobalConsts.ALLOWED_NOTICES_NUMBERS)
            {
                string expectedParameters = string.Concat(
                    $"bib.author all \"pokemon\" or bib.title all \"pokemon\" ",
                    $"or bib.isbn all \"pokemon\" ",
                    $"&recordSchema=unimarcxchange&maximumRecords={noticesQty}&startRecord=1"
                );

                string apiUrl = BnfApi.BnfApiStaticManager.GetSimpleSearchConditions(criterion, noticesQty);

                Assert.AreEqual(expectedParameters, apiUrl);
            }
        }

        /// <summary>
        /// The notices quantity is excluded from the list : 20, 100, 200, 500, 1000
        /// ---> Should use the default value 20
        /// </summary>
        [TestMethod]
        public void WithAnIrrelevantNoticesQuantity()
        {
            string criterion = "narnia";
            int noticesQty = 54;

            string expectedParameters = string.Concat(
                $"bib.author all \"narnia\" or bib.title all \"narnia\" ",
                $"or bib.isbn all \"narnia\" ",
                $"&recordSchema=unimarcxchange&maximumRecords=20&startRecord=1"
            );

            string apiUrl = BnfApi.BnfApiStaticManager.GetSimpleSearchConditions(criterion, noticesQty);

            Assert.AreEqual(expectedParameters, apiUrl);
        }

        [TestMethod]
        public void SeveralWordsWithinTheCriterion()
        {
            string criterion = "attaque des titans";
            int noticesQty = 200;
            string expectedParameters = string.Concat(
                $"bib.author all \"attaque+des+titans\" or bib.title all \"attaque+des+titans\" ",
                $"or bib.isbn all \"attaque+des+titans\" ",
                $"&recordSchema=unimarcxchange&maximumRecords=200&startRecord=1"
            );

            string apiUrl = BnfApi.BnfApiStaticManager.GetSimpleSearchConditions(criterion, noticesQty);

            Assert.AreEqual(expectedParameters, apiUrl);
        }
    }
}