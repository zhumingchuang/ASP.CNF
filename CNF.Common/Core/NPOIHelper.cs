using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;

namespace CNF.Common.Core;

public class NPOIHelper
{
    public static async Task<string> GenerateImportTemplate<T>() where T : class, new()
    {
        IImporter Importer = new ExcelImporter();
        if (!Directory.Exists("wwwroot"))
        {
            Directory.CreateDirectory("wwwroot");
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImportTemplate.xlsx");
        if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
        var result = await Importer.GenerateTemplate<T>(filePath);
        // File(System.IO.File.ReadAllBytes(result.FileName), "application/vnd.ms-excel", "关键词.xlsx");
        return result.FileName;
    }

    public static async Task<List<T>> Import<T>(IFormFile file) where T : class, new()
    {
        var extensionName = Path.GetExtension(file.FileName);
        if (!extensionName.ToLower().Equals(".xlsx"))
        {
            throw new Exception("文件格式不正确,只支持XLSX文件！");
        }

        using (var stream = file.OpenReadStream())
        {
            IImporter Importer = new ExcelImporter();
            var import = await Importer.Import<T>(stream);
            if (import?.Exception != null)
            {
                throw new Exception($"数据导入失败了！{import?.Exception}");
            }

            if (import.Data.Count > 0)
            {
                var lists = import.Data.ToList();
                return lists;
                //if (lists.Count > 0)
                //{
                //    var repository = MyHttpContext.Current.RequestServices.GetRequiredService<IBaseRepository<T>>();
                //    await repository.AddListAsync(lists);
                //}
            }
        }

        return null;
    }
}