using System;

namespace StockTicker.Lib.DAL.EF
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IEntity<TId> where TId : IComparable
    {

        TId Id { get; set; }

        DateTime CreatedDate { get; set; }

        string CreatedBy { get; set; }

        DateTime? UpdatedDate { get; set; }

        string UpdatedBy { get; set; }

        bool IsDeleted { get; set; }

    } 


}