using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NLog;
using eExtractor.Parser;

using eExtractor.Test.Entity;
using eExtractor.Util;

namespace eExtractor.Test
{
    public class Pdf1099KExtractor
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string _folder;
        private string _pdfPattern;
        private string _year;
        private RegexParser _parser;
        /// <summary>
        /// specify folder and year to extractor
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="year"></param>
        public Pdf1099KExtractor(string folder, string year)
        {
            _folder = folder;
            _year = year;
            _pdfPattern = AppConfig.pdf1099k;
            _parser = RegexParser.Build(AppConfig.f1099kRulePath);
        }
        /// <summary>
        /// <strong> only release mode will ignore other will throw exception </strong>
        /// </summary>
        private Pdf1099K ExtractFile(KeyValuePair<string, string> file, Extract1099KEntities ctx)
        {
            Pdf1099K file1099k = null;
#if (!DEBUG)
            try
            {
#endif
                string content = FileUtil.GetPdfContent(file.Value);
                // file1099k = File1099KParser.Parse(content);
                file1099k = _parser.Parse<Pdf1099K>(content);

                file1099k.FileName = file.Key;
                file1099k.FileLocation = file.Value;
                file1099k.PDFContent = File.ReadAllBytes(file.Value);
                file1099k.Save(ctx, "Steven Wu");
                logger.Info($"File {file.Key} extract succeed! ID: {file1099k.ID}");
#if (!DEBUG)
            }

            catch (Exception ex)
            {
                logger.Info($"Extract file {file.Key} failed check log file for more detail!");
                logger.Error(ex, $"=========== Extract *{file.Key}* failed exception===========");
                ctx.SaveChanges();
            }
#endif
            return file1099k;
        }

        public List<Pdf1099K> Extract()
        {
            // steps was follow up
            // read files under folder with patten
            // call 1099 k file  parser to parse from get data to fields
            // save PDF file to database return saved file with IDs
            using (var ctx = new Extract1099KEntities())
            {
                Dictionary<string, string> totalFiles = FileUtil.Search(_folder, _pdfPattern);
                List<Pdf1099K> files = totalFiles
                                       .Select((kv) => ExtractFile(kv, ctx))
                                       .Where(f => f != null)
                                       .ToList();

                ctx.SaveChanges();
                logger.Info($"=== Extract {files.Count}/{totalFiles.Count}(succeed/total) files===");
                return files;
            }


        }


    }
}
