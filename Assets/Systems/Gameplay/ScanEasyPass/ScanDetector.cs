using UnityEngine;

public class ScanDetector : MonoBehaviour
{
    [SerializeField] private ScanManager scanManager;
    [SerializeField] private float scanTime;

    private float _currentScanTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Chip"))
            return;

        if (scanManager.hasScanned)
            return;

        scanManager.AnimText(true, scanTime);

        _currentScanTime += Time.deltaTime;

        if (_currentScanTime >= scanTime)
        {
            scanManager.CompleteChip();

            scanManager.hasScanned = true;

            _currentScanTime = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Chip"))
            return;

        _currentScanTime = 0;

        if (!scanManager.hasScanned)
            scanManager.AnimText(false, scanTime);
    }
}
