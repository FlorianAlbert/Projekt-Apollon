namespace Apollon.Mud.Server.Model.Interfaces
{
    /// <summary>
    /// Choosable property of an avatar
    /// </summary>
    public interface IChoosable : IApprovable
    {
        /// <summary>
        /// Name of the property
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Description of the property
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Health value of the property
        /// </summary>
        int DefaultHealth { get; set; }

        /// <summary>
        /// Protection value of the property
        /// </summary>
        int DefaultProtection { get; set; }

        /// <summary>
        /// Damage value of the property
        /// </summary>
        int DefaultDamage { get; set; }
    }
}
