// namespace CNF.Infrastructure;
//
// public class EntityExpand
// {
//     /// <summary>
//     /// 显示错层方法
//     /// </summary>
//     public static string LevelName(string name, decimal? level)
//     {
//         if (level > 1)
//         {
//             string nbsp = "";
//             for (int i = 0; i < level; i++)
//             {
//                 nbsp += "　";
//             }
//
//             name = nbsp + "|--" + name;
//         }
//
//         return name;
//     }
//
//     /// <summary>
//     /// 处理数据库树形结构数据。比如menu\category\column
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <param name="parentId">树形父id</param>
//     /// <param name="id">树形id</param>
//     /// <param name="getDataCallback">回调函数</param>
//     /// <returns></returns>
//     public static async Task<Tuple<int, string>> DealTreeData<T>(int parentId, int id, Func<Task<T>> getDataCallback)
//         where T : BaseTenantTreeEntity
//     {
//         string parentIdList = "";
//         int layer = 0;
//         if (parentId > 0)
//         {
//             // 说明有父级  根据父级，查询对应的模型
//             var model = await getDataCallback();
//             if (model?.Id > 0)
//             {
//                 parentIdList = model.ParentList + id + ",";
//                 layer = model.Layer + 1;
//             }
//         }
//         else
//         {
//             parentIdList = "," + id + ",";
//             layer = 1;
//         }
//
//         return new Tuple<int, string>(layer, parentIdList);
//     }
//
//     /// <summary>
//     /// 树形数据递归 比如menu\category\column
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <param name="list"></param>
//     /// <param name="newlist"></param>
//     /// <param name="parentId"></param>
//     public static void ChildNode<T>(List<T> list, List<T> newlist, int parentId) where T : BaseTenantTreeEntity
//     {
//         var result = list.Where(p => p.ParentId == parentId).OrderBy(p => p.Layer).ToList();
//         /*主要解决类似树形结构里面当前的元素的parentId存在且在数据库不存在，导致查询parentId=0判断时，该元素包含到集合。*/
//
//         if (parentId == 0)
//         {
//             foreach (var item in list)
//             {
//                 if (item.ParentId == 0)
//                 {
//                     continue;
//                 }
//
//                 T model = list.FirstOrDefault(d => d.Id == item.ParentId);
//                 if (model == null)
//                 {
//                     result.Add(item);
//                 }
//             }
//         }
//
//         if (!result.Any()) return;
//         for (int i = 0; i < result.Count; i++)
//         {
//             newlist.Add(result[i]);
//             ChildNode(list, newlist, result[i].Id);
//         }
//     }
// }