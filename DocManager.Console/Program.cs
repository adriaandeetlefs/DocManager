using DocManager.Client.Helpers;
using DocManager.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DocManager.Client
{
    class Program
    {
        // !Feedback: Rather use DI for dependencies
        private static APIHelper apiHelper { get; set; }
        static void Main(string[] args)
        {
            apiHelper = new APIHelper("https://localhost:44352");

            //Prompt user with options
            ShowOptions();
            BasicDecisionMaker();
        }

        private static void ShowOptions()
        {
            Console.WriteLine("");
            Console.WriteLine("Please input a selection with the Number of the option:");
            Console.WriteLine("1. Upload Document");
            Console.WriteLine("2. Download Document");
            Console.WriteLine("3. Exit");
            Console.WriteLine("");
            // Feedback *1
            //BasicDecisionMaker()
        }

        // Feedback *1: Move calls to this method inside ShowOptions
        private static void BasicDecisionMaker()
        {
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                case "2":
                case "3":
                    SelectingOptions(input);
                    return;
                default:
                    Console.WriteLine("Incorrect option selected, please type in only the number of the option selected");
                    break;
            }

            ShowOptions();
            // Feedback *1
            BasicDecisionMaker();
        }

        private static async void SelectingOptions(string option)
        {
            try
            {
                switch (option)
                {
                    case "1":
                        Console.WriteLine("To upload a document please type in the absolute path of the file and continue.");
                        var input = Console.ReadLine();
                        /* Read the File Contents and populate model to send to api */
                        // !Feedback: Opinionated : use var
                        Tuple<bool, string> result = await apiHelper.UploadDocument(input);
                        // !Feedback: Opinionated : use {} for if or replace with ternary operation
                        if (result.Item1)
                            Console.WriteLine("File Uploaded successfully");
                        else
                            Console.WriteLine("File Upload was unsuccessful");

                        ShowOptions();
                        // Feedback *1
                        BasicDecisionMaker();
                        break;
                    case "2":
                        // First list the documents that are available to download
                         // !Feedback: Opinionated : use var
                        Tuple<bool,List<DocumentModel>> docListResult = await apiHelper.GetDocumentList();

                        if (docListResult.Item1)
                        {
                            Console.WriteLine("List of all documents available to download:");
                             // !Feedback: Opinionated : use string.Empty
                            Console.WriteLine("");
                             // !Feedback: Opinionated : use {}
                            foreach (var doc in docListResult.Item2)
                                 // !Feedback: use string interpolation instead
                                Console.WriteLine("" + doc.FileName + "");

                            Console.WriteLine("");
                            Console.WriteLine("To download a document please type in the name of the file and continue.");
                            input = Console.ReadLine();

                            if (input.Length > 0)
                            {
                                //Download the document result 
                                Tuple<bool, string> docDownloadResult = await apiHelper.DownloadDocument(input);

                                if (docDownloadResult.Item1)
                                {
                                    Console.WriteLine("Document Successfully downloaded");
                                    ShowOptions();
                                    // Feedback *1
                                    BasicDecisionMaker();

                                }
                                else
                                {
                                    Console.WriteLine("Document could not be downloaded");
                                    ShowOptions();
                                    // Feedback *1
                                    BasicDecisionMaker();
                                }
                            }
                        }
                        else
                            Console.WriteLine("Retrieval of document listing failed");

                        ShowOptions();
                        // Feedback *1
                        BasicDecisionMaker();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                // !Feedback: use string interpolation instead
                Console.WriteLine("Something went wrong: "+ex.Message);
                Console.WriteLine("Please try again...");
                Console.WriteLine("");
                ShowOptions();
                // Feedback *1
                BasicDecisionMaker();
            }
        }
    }
}
