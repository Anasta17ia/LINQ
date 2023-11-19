﻿using ConsoleApp.Model;
using ConsoleApp.Model.Enum;
using ConsoleApp.OutputTypes;

namespace ConsoleApp;

public class QueryHelper : IQueryHelper
{
    /// <summary>
    /// Get Deliveries that has payed
    /// </summary>
    public IEnumerable<Delivery> Paid(IEnumerable<Delivery> deliveries) => deliveries.Where(d=>d.PaymentId!=null); //TODO: Завдання 1

    /// <summary>
    /// Get Deliveries that now processing by system (not Canceled or Done)
    /// </summary>
    public IEnumerable<Delivery> NotFinished(IEnumerable<Delivery> deliveries) => deliveries.Where(d=>d.Status!=DeliveryStatus.Canselled&&d.Status!=DeliveryStatus.Done); //TODO: Завдання 2
    
    /// <summary>
    /// Get DeliveriesShortInfo from deliveries of specified client
    /// </summary>
    public IEnumerable<DeliveryShortInfo> DeliveryInfosByClient(IEnumerable<Delivery> deliveries, string clientId) => deliveries.Where(d=>d.ClientId==clientId).Select(d=>new DeliveryShortInfo()
    {
        ArrivalPeriod = d.ArrivalPeriod,
        CargoType = d.CargoType,
        ClientId = clientId,
        StartCity = d.Direction.Origin.City,
        EndCity = d.Direction.Destination.City,
        Id = d.Id,
        LoadingPeriod = d.LoadingPeriod,
        Status = d.Status,
        Type = d.Type
    }); //TODO: Завдання 3
    
    /// <summary>
    /// Get first ten Deliveries that starts at specified city and have specified type
    /// </summary>
    public IEnumerable<Delivery> DeliveriesByCityAndType(IEnumerable<Delivery> deliveries, string cityName, DeliveryType type) => 
    deliveries.Where(d => d.Direction.Origin.City == cityName && d.Type == type);//TODO: Завдання 4
    
    /// <summary>
    /// Order deliveries by status, then by start of loading period
    /// </summary>
    public IEnumerable<Delivery> OrderByStatusThenByStartLoading(IEnumerable<Delivery> deliveries) => deliveries.OrderBy(d => d.Status).ThenBy(d => d.LoadingPeriod.Start);//TODO: Завдання 5

    /// <summary>
    /// Count unique cargo types
    /// </summary>
    public int CountUniqCargoTypes(IEnumerable<Delivery> deliveries) => deliveries.Select(d => d.CargoType).Distinct().Count(); //TODO: Завдання 6
    
    /// <summary>
    /// Group deliveries by status and count deliveries in each group
    /// </summary>
    public Dictionary<DeliveryStatus, int> CountsByDeliveryStatus(IEnumerable<Delivery> deliveries) => deliveries.GroupBy(d => d.Status).ToDictionary(g => g.Key, g => g.Count());//TODO: Завдання 7
    
    /// <summary>
    /// Group deliveries by start-end city pairs and calculate average gap between end of loading period and start of arrival period (calculate in minutes)
    /// </summary>
    public IEnumerable<AverageGapsInfo> AverageTravelTimePerDirection(IEnumerable<Delivery> deliveries) => 
    deliveries.GroupBy(d => new{ StartCity = d.Direction.Origin.City, EndCity = d.Direction.Destination.City }) .Select(g => new AverageGapsInfo
    {
        StartCity = g.Key.StartCity,
        EndCity = g.Key.EndCity,
        AverageGap = g.Average(d =>(d.ArrivalPeriod.Start.Value - d.LoadingPeriod.End.Value).Minutes)
    });//TODO: Завдання 8

    /// <summary>
    /// Paging helper
    /// </summary>
    public IEnumerable<TElement> Paging<TElement, TOrderingKey>(IEnumerable<TElement> elements,
        Func<TElement, TOrderingKey> ordering,
        Func<TElement, bool>? filter = null,
        int countOnPage = 100,
        int pageNumber = 1)
        {
            if(filter!=null)
            {
                elements=elements.Where(filter);
            }
            return elements.OrderBy(ordering).Skip((pageNumber - 1) * countOnPage).Take(countOnPage);
        } //TODO: Завдання 9 
}
