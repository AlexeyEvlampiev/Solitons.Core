using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Management.Postgres;

/// <summary>
/// Provides functionality for managing a Postgres database, enabling declarative and idempotent database operations.
/// </summary>
/// <remarks>
/// This interface defines methods for creating, deleting, and testing a Postgres database. It supports the concept of 'Desired State Configuration' wherein the <see cref="CreateDbAsync"/> method ensures the database is set up according to the desired state. This can include creating necessary roles, registering secrets, setting role permissions, populating the database with reference data, and more.
/// The <see cref="DropDbAsync"/> method allows for complete removal of the database, useful in scenarios like DevOps and Integration Tests where databases may need to be provisionally created and then removed.
/// The <see cref="PerformPostUpgradeTestsAsync"/> method encourages a model where the database is treated as software and tests are run to ensure its proper functioning. While this approach may not suit all, the interface provides sufficient flexibility for a wide range of use-cases.
/// </remarks>
public interface IPgManager
{
    private static readonly Regex DatabaseNameRegex = new Regex(@"^(?i)[a-z_]\w{1,62}$");
    private static readonly Regex RoleNameRegex = new Regex(@"^(?i)[a-z_][\w$]{0,62}$");

    /// <summary>
    /// Gets the name of the database that this instance manages.
    /// </summary>
    string Database { get; }

    /// <summary>
    /// Creates or updates the database to a desired state asynchronously.
    /// </summary>
    /// <remarks>
    /// This method adheres to the Desired State Configuration (DSC) principle and is idempotent. 
    /// If the database does not exist, this method will create it on the target Postgres server. 
    /// If the database already exists, this method will upgrade it and configure it according to the desired state. 
    /// The primary outcome of this method is the creation or configuration of the database on the server. 
    /// A comprehensive implementation of this method might include creating all necessary roles, 
    /// registering the corresponding secrets (such as connection strings) in a selected secrets repository,
    /// setting role permissions, populating the database with reference data, and more.
    /// </remarks>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateDbAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Drops the database asynchronously.
    /// </summary>
    /// <remarks>
    /// This method is useful for advanced DevOps and Integration Testing scenarios where a database is provisioned temporarily on a target Postgres server and needs to be deleted after operations are completed.
    /// A comprehensive implementation of this method should also handle the removal of all database-specific roles and permissions upon deletion of the database.
    /// </remarks>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DropDbAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Executes a series of tests on the database asynchronously.
    /// </summary>
    /// <remarks>
    /// These tests aim to validate the correctness of the various functionalities of the database, which may include stored procedures, if they are used. This methodology is rooted in the philosophy that databases can be a robust location for business logic, with other parts of the system acting primarily as middleware or a database client. This perspective is not universally shared, and as such, the actual implementation of tests can and should be adapted to the unique requirements and philosophies of each engineering team and project.
    /// </remarks>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PerformPostUpgradeTestsAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Drops and recreates the database asynchronously.
    /// </summary>
    /// <remarks>
    /// This method provides an easy way to reset the database to a clean state, which is particularly useful in testing or development scenarios where you may need to operate on a fresh instance of the database.
    /// It's a shortcut method that first calls <see cref="DropDbAsync"/> to delete the database and all its associated roles and permissions, and then calls <see cref="CreateDbAsync"/> to recreate the database, set it up according to the desired state, and establish necessary roles and permissions.
    /// This operation might involve creating roles, registering secrets, setting role permissions, populating the database with reference data and more.
    /// 
    /// <strong>Warning:</strong> Executing this method will cause all data in the database to be permanently lost. Ensure to back up any important data before invoking this operation.
    /// </remarks>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [DebuggerStepThrough]
    public sealed async Task DropAndRecreateAsync(CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        await DropDbAsync(cancellation);
        cancellation.ThrowIfCancellationRequested();
        await CreateDbAsync(cancellation);
    }


    /// <summary>
    /// Determines whether the provided string is a valid Postgres database name.
    /// </summary>
    /// <param name="databaseName">The name of the database to validate.</param>
    /// <returns>
    /// <see langword="true"/> if the provided database name is valid; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// A valid Postgres database name must meet the following criteria:
    /// - It must start with a letter or an underscore.
    /// - It can contain letters, digits, and underscores.
    /// - It cannot be longer than 63 characters.
    /// This method uses case-insensitive matching (that is, "myDatabase" and "MYDATABASE" are considered the same).
    /// </remarks>
    [DebuggerNonUserCode]
    public static bool IsValidDatabaseName(string databaseName) => DatabaseNameRegex.IsMatch(databaseName);

    /// <summary>
    /// Determines whether the provided string is a valid Postgres role name.
    /// </summary>
    /// <param name="roleName">The name of the role to validate.</param>
    /// <returns>
    /// <see langword="true"/> if the provided role name is valid; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// A valid Postgres role name must meet the following criteria:
    /// - It must start with a letter or an underscore.
    /// - It can contain letters, digits, underscores, and dollar signs.
    /// - It cannot be longer than 63 bytes.
    /// This method uses case-insensitive matching (that is, "myRole" and "MYROLE" are considered the same).
    /// </remarks>
    [DebuggerNonUserCode]
    public static bool IsValidRoleName(string roleName) => RoleNameRegex.IsMatch(roleName);
}