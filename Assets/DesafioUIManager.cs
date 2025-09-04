using UnityEngine;
using UnityEngine.UI;

public class DesafioUIManager : MonoBehaviour
{
    [Header("Toggles de desafíos")]
    public Toggle recreoCanceladoToggle;
    public Toggle maestroEstrictoToggle;
    public Toggle clasesNocturnasToggle;
    public Toggle tintaExplosivaToggle;
    public Toggle odioLosLunesToggle;
    public Toggle aprobadoCon10Toggle;
    public Toggle apuntesCompletosToggle;
    public Toggle meriendaExtraToggle;
    public Toggle comodinEscolarToggle;

    void Start()
    {
        // Inicializar estado de cada toggle con lo guardado en PlayerPrefs
        recreoCanceladoToggle.isOn = DesafioManager.Instance.GetDesafio("RecreoCancelado");
        maestroEstrictoToggle.isOn = DesafioManager.Instance.GetDesafio("MaestroEstricto");
        clasesNocturnasToggle.isOn = DesafioManager.Instance.GetDesafio("ClasesNocturnas");
        tintaExplosivaToggle.isOn = DesafioManager.Instance.GetDesafio("TintaExplosiva");
        odioLosLunesToggle.isOn = DesafioManager.Instance.GetDesafio("OdioLosLunes");
        aprobadoCon10Toggle.isOn = DesafioManager.Instance.GetDesafio("AprobadoCon10");
        apuntesCompletosToggle.isOn = DesafioManager.Instance.GetDesafio("ApuntesCompletos");
        meriendaExtraToggle.isOn = DesafioManager.Instance.GetDesafio("MeriendaExtra");
        comodinEscolarToggle.isOn = DesafioManager.Instance.GetDesafio("ComodinEscolar");

        // Listener para que cuando el jugador clickee se guarde en el manager
        recreoCanceladoToggle.onValueChanged.AddListener((value) => DesafioManager.Instance.SetDesafio("RecreoCancelado", value));
        maestroEstrictoToggle.onValueChanged.AddListener((value) => DesafioManager.Instance.SetDesafio("MaestroEstricto", value));
        clasesNocturnasToggle.onValueChanged.AddListener((value) => DesafioManager.Instance.SetDesafio("ClasesNocturnas", value));
        tintaExplosivaToggle.onValueChanged.AddListener((value) => DesafioManager.Instance.SetDesafio("TintaExplosiva", value));
        odioLosLunesToggle.onValueChanged.AddListener((value) => DesafioManager.Instance.SetDesafio("OdioLosLunes", value));
        aprobadoCon10Toggle.onValueChanged.AddListener((value) => DesafioManager.Instance.SetDesafio("AprobadoCon10", value));
        apuntesCompletosToggle.onValueChanged.AddListener((value) => DesafioManager.Instance.SetDesafio("ApuntesCompletos", value));
        meriendaExtraToggle.onValueChanged.AddListener((value) => DesafioManager.Instance.SetDesafio("MeriendaExtra", value));
        comodinEscolarToggle.onValueChanged.AddListener((value) => DesafioManager.Instance.SetDesafio("ComodinEscolar", value));
    }
}
