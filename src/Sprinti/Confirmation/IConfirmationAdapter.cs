using Sprinti.Domain;

namespace Sprinti.Confirmation;

public interface IConfirmationAdapter
{
    /**
     * Übermittelt den Start eines neuen Durchlaufs von Team XX, XX bezeichnet die zweistellige Nummer des Teams.
     * Es müssen keine Daten im Body der Meldung mitgeschickt werden.
     * Als Startzeit wird der Zeitpunkt genommen, bei dem der Request auf dem Server eintrifft.
     * Beim Senden dieses Requests wird die aktuelle Konfiguration und der Endzeitpunkt in der Datenbank gelöscht.
     */
    Task StartAsync(CancellationToken cancellation);

    /**
     * Übermittlung des Endes des Durchlaufs. Es müssen keine Daten mitgeschickt werden.
     */
    Task EndAsync(CancellationToken cancellation);

    /**
     * Übermittlung der erkannten Konfiguration der Würfelanordnung anhand des vorgegebenen Schemas.
     */
    Task ConfirmAsync(CubeConfig config, CancellationToken cancellation);

    /**
     * Es wird keine Authentisierung verwendet.
     * Diese URL kann zum Testen der Erreichbarkeit des Applikations-Servers benutzt werden.
     * Der Server antwortet mit "Hello" und Status Code 200.
     * Es wird keine Authentisierung verwendet.
     */
    Task<bool> HealthCheckAsync(CancellationToken cancellation);
}