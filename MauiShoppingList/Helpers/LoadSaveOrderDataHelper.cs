﻿using Newtonsoft.Json;
using Test_MauiApp1;
using Test_MauiApp1.Models;
using Test_MauiApp1.Services;

namespace Test_MauiApp1.Helpers
{
    public class LoadSaveOrderDataHelper
    {
        /// <summary>
        /// ////////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        public static void LoadListAggregatorsOrder(StateService stateService)
        {
           
            if (stateService.StateInfo.User == null || stateService.StateInfo.User.ListAggregators == null || !stateService.StateInfo.User.ListAggregators.Any()) return;


            SetEntryOrder2(stateService.StateInfo.User.ListAggregators);

            string UserName = "";
            if (Preferences.Default.ContainsKey("UserName"))
                UserName = Preferences.Default.Get("UserName","");

            if (!Preferences.Default.ContainsKey($"{UserName}ListOrder"))
            {
                SaveAllOrder(stateService.StateInfo.User.ListAggregators);
            }
            List<OrderListAggrItem> tempListFromFile = null;

            try
            {
                var toDeserialize = Preferences.Default.Get($"{UserName}ListOrder","");

                tempListFromFile = JsonConvert.DeserializeObject<List<OrderListAggrItem>>(toDeserialize);
            }
            catch
            {

            }
            // if (tempListFromFile == null) return;  //????????????????? nie


            foreach (var listAggr in stateService.StateInfo.User.ListAggregators)
            {

                var itemAggrFromFile = tempListFromFile?.Where(a => a.Id == listAggr.ListAggregatorId).FirstOrDefault();

                if (itemAggrFromFile != null)
                {

                    listAggr.Order = itemAggrFromFile.Order;

                }


                ////////////////////
                SetEntryOrder2(listAggr.Lists);


                foreach (var listList in listAggr.Lists)
                {

                    var itemListFromFile = itemAggrFromFile?.List.Where(a => a.Id == listList.ListId).FirstOrDefault();

                    if (itemListFromFile != null)
                    {

                        listList.Order = itemListFromFile.Order;

                    }


                    SetEntryOrder2(listList.ListItems);


                    foreach (var listItem in listList.ListItems)
                    {

                        var itemItemFromFile = itemListFromFile?.List.Where(a => a.Id == listItem.ListItemId).FirstOrDefault();

                        if (itemItemFromFile != null)
                        {

                            listItem.Order = itemItemFromFile.Order;

                        }

                    }

                    ResolveDoubleOrderValue(listList.ListItems);

                    listList.ListItems = listList.ListItems.OrderByDescending(a => a.Order).ToList();
                }

                ResolveDoubleOrderValue(listAggr.Lists);

                listAggr.Lists = listAggr.Lists.OrderByDescending(a => a.Order).ToList();
                ////////////////////////////


            }


            ResolveDoubleOrderValue(stateService.StateInfo.User.ListAggregators);


            stateService.StateInfo.User.ListAggregators = stateService.StateInfo.User.ListAggregators.OrderByDescending(a => a.Order).ToList();

        }
              

        static void ResolveDoubleOrderValue(IEnumerable<IModelItemOrder> list)
        {
            foreach (var item in list)
            {
                foreach (var item2 in list)
                {
                    if (item.Id != item2.Id)
                        if (item.Order == item2.Order)
                            item2.Order = list.Max(a => a.Order) + 1;
                }
            }

        }

        static void SetEntryOrder(IEnumerable<IModelItemOrder> list)
        {
            int i = 1;
            foreach (var item in list)
            {
                //if (item.Order == 0)
                //{
                //    item.Order = list.Max(a => a.Order) + 1;

                //}

                item.Order = i++;
            }

        }
        static void SetEntryOrder2(IEnumerable<IModelItemOrder> list)
        {
            int i = 1;
            foreach (var item in list)
            {

                item.Order = item.Id;
            }

        }
        public static void SaveAllOrder(ICollection<ListAggregator> list)
        {
            var tempAggrList = new List<OrderListAggrItem>();

            int i = 1, j = 1, k = 1;
            i = list.ToList().Count;
            list.ToList().ForEach(aggr =>
            {

                var itemAggr = new OrderListAggrItem { Id = aggr.ListAggregatorId, Order = i-- };

                j = aggr.Lists.Count;
                aggr.Lists.ToList().ForEach(lista =>
                {
                    var itemList = new OrderListItem { Id = lista.ListId, Order = j-- };

                    k = lista.ListItems.Count;
                    lista.ListItems.ToList().ForEach(item =>
                    {

                        var itemItem = new OrderItem { Id = item.ListItemId, Order = k-- };

                        itemList.List.Add(itemItem);

                    });


                    itemAggr.List.Add(itemList);
                });


                tempAggrList.Add(itemAggr);

            });




            if (Preferences.Default.ContainsKey("UserName"))
            {
                var UserName =  Preferences.Default.Get("UserName","");

                string serializedObject = JsonConvert.SerializeObject(tempAggrList);

                Preferences.Default.Set($"{UserName}ListOrder", serializedObject);

            }
        }
    }
}
