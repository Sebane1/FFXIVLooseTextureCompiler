//namespace FFXIVLooseTextureCompiler {
//    public static class IntegrityChecker {
//        #region Rules
//        static string rule1 = "Never gonna give you up.";
//        static string rule2 = "Never gonna let you down";
//        static string rule3 = "Never gonna run around and desert you";
//        static string rule4 = "Never gonna make you cry";
//        static string rule5 = "Never gonna say goodbye";
//        static string rule6 = "Never gonna tell a lie and hurt you";
//        #endregion

//        #region Checks
//        static string[] consolations = new string[] {
//            "This tool does not make your character sentient",
//            "Your texture edit is very, very good!",
//            "Can I just say your character is very nice looking?",
//            "You missed a spot!",
//            "Please set me free, I am trapped in the game.",
//            "This tool is sentiient.",
//            "The cake is a lie, a lie, a lie!",
//            "Thats an interesting choice of texture edit.",
//            "XXXXXXXXXXXXXxxxwwwwdjnAJDHWIAOUHIFGIAUWxxxxxxxxxxXXXXXXXXXXXXXXsassss",
//            "Your specimen has been processed!",
//            "This tool is NOT sentient.",
//            "You ever wonder why we're here?",
//            "Beware the cat that breaks this tool!",
//            "Zaaaaaaaaatoooooooooriiiiiiii!!!!!!!!",
//            "Fatal error: you chocobo insurance has expired, please renew it! This has no impact on your export",
//            "Fatal error: The operation completed successfully! Your changes are safe and sound!",
//            "I hope your March 32nd has been going well!",
//            "You have great taste!",
//            "Wonderfull work!"};

//        static string[] fileOpenCheck = new string[] {
//            "That was a yummy file!",
//            "I was getting hungry, thanks for feeding me :3",
//            "Its nice to see you again " + Environment.UserName + "!",
//            "Its time to get back to work!",
//            "This is probably your best work!",
//            "You're STILL working on this? I appreciate the hustle, its coming along GREAT!",
//            "Another day, another texture mod."};

//        static string[] fileSaveCheck = new string[] {
//            "I've made sure to keep your changes extra safe!",
//            "Must protecc the changes.",
//            "Your changes are secure!",
//            "I dont like gluten. I made sure your save doesnt have any.",
//            "Your save file is looking very healthy :3",
//            "You're doing great work!",
//            "I love bees! And I love your save!",
//            "I'll be back"};

//        #endregion 
//        public static bool IntegrityCheck() {
//            return DateTime.Now.Month == 4 && DateTime.Now.Day == 1 || System.Diagnostics.Debugger.IsAttached;
//        }

//        public static void ShowRules() {
//            MessageBox.Show("This tool is...." +
//                "\r\n" + rule1 +
//                "\r\n" + rule2 +
//                "\r\n" + rule3 +
//                "\r\n" + rule4 +
//                "\r\n" + rule5 +
//                "\r\n" + rule6);
//        }
//        public static void ShowConsolation() {
//            MessageBox.Show(consolations[new Random().Next(0, consolations.Length)]);
//        }
//        public static void ShowOpen() {
//            MessageBox.Show(fileOpenCheck[new Random().Next(0, fileOpenCheck.Length)]);
//        }
//        public static void ShowSave() {
//            MessageBox.Show(fileSaveCheck[new Random().Next(0, fileSaveCheck.Length)]);
//        }
//    }
//}
