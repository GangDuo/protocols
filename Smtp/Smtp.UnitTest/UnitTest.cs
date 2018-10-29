using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace Smtp.UnitTest
{
    /// <summary>
    /// UnitTest2 の概要の説明
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        public UnitTest()
        {
            //
            // TODO: コンストラクター ロジックをここに追加します
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        //
        // テストを作成する際には、次の追加属性を使用できます:
        //
        // クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 各テストを実行する前に、TestInitialize を使用してコードを実行してください
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ShouldBeSuccessfulWhenSendingMail()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var smtpHost = appSettings["unitTest:smtpHost"];
            var user = appSettings["unitTest:smtpAuthUser"];
            var pass = appSettings["unitTest:smtpAuthPass"];
            var sender = appSettings["unitTest:emailAddressSender"];
            var to = appSettings["unitTest:commaSeparatedEmailAddressRecipientsTo"].Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            Assert.IsTrue(smtpHost.Length > 0);
            Assert.IsTrue(user.Length > 0);
            Assert.IsTrue(pass.Length > 0);
            Assert.IsTrue(sender.Length > 0);
            Assert.IsTrue(to.Length > 0);
        }
    }
}
