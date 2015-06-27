using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Reporting.WebForms;

namespace KNPX.DeliveryServices.Framework.Util
{
    /// <summary>
    /// Classe utilitária para geração de relatórios.
    /// </summary>
    public class ReportGenerator
    {
        private static List<ReportDataSource> _subReportDataSources;

        /// <summary>
        /// Tipo de Relatório.
        /// </summary>
        public enum ReportTypeEnum
        {
            /// <summary>
            /// PDF.
            /// </summary>
            Pdf = 1,
            /// <summary>
            /// Excel.
            /// </summary>
            Excel = 2,
            /// <summary>
            /// Word. (.doc)
            /// </summary>
            Word = 3,
            /// <summary>
            /// Imagem. (.png)
            /// </summary>
            Image = 4
        }

        /// <summary>
        /// Gera um relatório com base nos parâmetros informados.
        /// </summary>
        /// <param name="reportTypeEnum">Tipo report que deve ser gerado. (PDF, Excel, Word e Imagem)</param>
        /// <param name="reportPath">Path completo do arquivo .rdlc do relatório.</param>
        /// <param name="reportDataSources">Lista de DataSource do relatório.</param>
        /// <param name="reportParameters">Lista de Parâmetros do relatório.</param>
        /// <param name="mimeType">[out] O tipo MIME do relatório gerado.</param>
        /// <returns>Array de bytes do relatório gerado.</returns>
        public static byte[] GenerateReport(ReportTypeEnum reportTypeEnum, string reportPath, List<ReportDataSource> reportDataSources, List<ReportParameter> reportParameters, out string mimeType)
        {
            string reportType;
            string deviceInfo;

            LocalReport lr = ProccessLocalReport(reportTypeEnum, reportPath, reportDataSources, reportParameters, out reportType,
                out deviceInfo);

            return RenderReport(lr, reportType, deviceInfo, out mimeType);
        }

        /// <summary>
        /// Gera um relatório com sub relatório base nos parâmetros informados.
        /// </summary>
        /// <param name="reportTypeEnum">Tipo report que deve ser gerado. (PDF, Excel, Word e Imagem)</param>
        /// <param name="reportPath">Path completo do arquivo .rdlc do relatório.</param>
        /// <param name="reportDataSources">Lista de DataSource do relatório.</param>
        /// <param name="reportParameters">Lista de Parâmetros do relatório.</param>
        /// <param name="subReportDataSources">Lista de DataSource do sub relatório.</param>
        /// <param name="mimeType">[out] O tipo MIME do relatório gerado.</param>
        /// <returns>Array de bytes do relatório gerado.</returns>
        public static byte[] GenerateReportWithSubReport(ReportTypeEnum reportTypeEnum, string reportPath, List<ReportDataSource> reportDataSources, List<ReportParameter> reportParameters, List<ReportDataSource> subReportDataSources, out string mimeType)
        {
            try
            {
                string reportType;
                string deviceInfo;

                LocalReport lr = ProccessLocalReport(reportTypeEnum, reportPath, reportDataSources, reportParameters, out reportType,
                    out deviceInfo);

                _subReportDataSources = subReportDataSources;

                lr.SubreportProcessing += SubreportProcessing;

                byte[] report = RenderReport(lr, reportType, deviceInfo, out mimeType);

                return report;
            }
            finally
            {
                _subReportDataSources = null;
            }
        }

        private static LocalReport ProccessLocalReport(ReportTypeEnum reportTypeEnum, string reportPath, IEnumerable<ReportDataSource> reportDataSources,
                                                         List<ReportParameter> reportParameters, out string reportType, out string deviceInfo)
        {
            LocalReport lr = new LocalReport();

            if (!File.Exists(reportPath))
                throw new Exception("Report não encontrado!");

            lr.ReportPath = reportPath;

            foreach (var reportDataSource in reportDataSources)
                lr.DataSources.Add(reportDataSource);

            if (reportParameters.Any())
                lr.SetParameters(reportParameters);

            switch (reportTypeEnum)
            {
                case ReportTypeEnum.Pdf:
                    reportType = "PDF";
                    break;
                case ReportTypeEnum.Excel:
                    reportType = "Excel";
                    break;
                case ReportTypeEnum.Word:
                    reportType = "Word";
                    break;
                case ReportTypeEnum.Image:
                    reportType = "Image";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reportTypeEnum");
            }

            deviceInfo = "<DeviceInfo>" +
                         "  <OutputFormat>" + reportType + "</OutputFormat>" +
                         "  <PageWidth>8.5in</PageWidth>" +
                         "  <PageHeight>11in</PageHeight>" +
                         "  <MarginTop>0.5in</MarginTop>" +
                         "  <MarginLeft>1in</MarginLeft>" +
                         "  <MarginRight>1in</MarginRight>" +
                         "  <MarginBottom>0.5in</MarginBottom>" +
                         "</DeviceInfo>";

            return lr;
        }

        private static byte[] RenderReport(LocalReport localReport, string reportType, string deviceInfo, out string mimeType)
        {
            string fileNameExtension;
            string encoding;
            Warning[] warnings;
            string[] streams;

            return localReport.Render(
               reportType,
               deviceInfo,
               out mimeType,
               out encoding,
               out fileNameExtension,
               out streams,
               out warnings);
        }

        static void SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            foreach (var subReportDataSource in _subReportDataSources)
                e.DataSources.Add(subReportDataSource);
        }
    }
}
