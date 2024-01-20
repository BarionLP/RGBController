namespace RGBController.Core;

public sealed class BeatDetector : IBeatDetector{
    private readonly FixedSizeBuffer<float> _energyBuffer;
    private readonly FixedSizeBuffer<float> _beatBuffer;
    public float BeatThreshold { get; set; } = 0.1f;
    public int MinBeatGap { get; set; } = 400;
    public int MinBeatWidth { get; set; } = 2;
    public int WindowSize { get; init; } = 50;
    public bool AutoGainEnabled { get; set; } = true;

    public float Gain { get; set; } = float.MaxValue;
    public DateTime LastBeatTime { get; set; }
    public event Action? OnBeatDetected;

    public BeatDetector(){
        _energyBuffer = new(WindowSize);
        _beatBuffer = new(WindowSize);
    }

    public void ProcessData(float[] samples){
        if(samples.Length == 0) return;
        
        float energy = 0;

        for (int i = 0; i < samples.Length; i++){
            energy += samples[i] * samples[i];
        }

        energy /= samples.Length;

        if (AutoGainEnabled){
            if (energy * Gain > 1){
                Gain = 1 / energy;
            }

            Gain *= 1.0005f;
        }

        energy *= Gain;

        _energyBuffer.Add(energy);

        // Detect beat
        if (CheckBeatWidth(MinBeatWidth) &&
            (DateTime.Now.Subtract(LastBeatTime).TotalMilliseconds >= MinBeatGap) &&
            energy > BeatThreshold)
        {
            LastBeatTime = DateTime.Now;
            OnBeatDetected?.Invoke();
            _beatBuffer.Add(energy);
        }
        else{
            _beatBuffer.Add(0);
        }
    }

    private bool CheckBeatWidth(int minWidth){
        var c = -0.0000015f * Variance(_energyBuffer) + 1.5142857f;
        var thres = c * _energyBuffer.Average();

        for (int i = 0; i < minWidth; i++){
            if (_energyBuffer[i] < thres) return false; 
        }

        return true;
    }

    private static float Variance(IEnumerable<float> buffer){
        float avg = buffer.Average();
        float var = 0;
        int count = 0;

        foreach (float f in buffer){
            var += (f - avg) * (f - avg);
            count++;
        }

        return var / count;
    }
}


public class BeatDetector2 : IBeatDetector{
    public event Action? OnBeatDetected;

    private const int EnergyHistoryLength = 43; // Adjust based on sample rate and buffer size
    private float[] energyHistory = new float[EnergyHistoryLength];
    private int energyHistoryIndex = 0;
    private const float C = 1.3f; // Sensitivity constant, adjust as needed

    public void ProcessData(float[] audioData)
    {
        float instantEnergy = CalculateInstantEnergy(audioData);
        float averageEnergy = CalculateAverageEnergy();

        if (IsBeatEnergy(instantEnergy, averageEnergy))
        {
            OnBeatDetected?.Invoke();
        }

        UpdateEnergyHistory(instantEnergy);
    }

    private float CalculateInstantEnergy(float[] audioData)
    {
        float energy = 0f;
        foreach (var sample in audioData)
        {
            energy += sample * sample;
        }
        return energy;
    }

    private float CalculateAverageEnergy()
    {
        float sum = 0f;
        foreach (var energy in energyHistory)
        {
            sum += energy;
        }
        return sum / energyHistory.Length;
    }

    private bool IsBeatEnergy(float instantEnergy, float averageEnergy)
    {
        return instantEnergy > C * averageEnergy;
    }

    private void UpdateEnergyHistory(float instantEnergy)
    {
        energyHistory[energyHistoryIndex] = instantEnergy;
        energyHistoryIndex = (energyHistoryIndex + 1) % energyHistory.Length;
    }
}

public sealed class BeatDetector3 : IBeatDetector{
    public event Action? OnBeatDetected;

    private readonly float[] _energyHistory;
    private int _energyHistoryIndex = 0;
    private float _gain = 1.0f;
    private const float GainIncreaseFactor = 1.0005f;
    private const float SensitivityConstant = 1.3f;
    public bool AutoGainEnabled { get; set; } = true;
    public int MinBeatGap { get; set; } = 400;
    private DateTime _lastBeatTime;

    public BeatDetector3(int historyLength = 43)
    {
        _energyHistory = new float[historyLength];
    }

    public void ProcessData(float[] audioData)
    {
        if (audioData.Length == 0) return;

        float instantEnergy = CalculateInstantEnergy(audioData);

        if (AutoGainEnabled)
        {
            AdjustGain(ref instantEnergy);
        }

        if (IsBeat(instantEnergy) &&
            (DateTime.Now - _lastBeatTime).TotalMilliseconds >= MinBeatGap)
        {
            _lastBeatTime = DateTime.Now;
            OnBeatDetected?.Invoke();
        }

        UpdateEnergyHistory(instantEnergy);
    }

    private static float CalculateInstantEnergy(float[] audioData){
        float energy = 0f;
        foreach (var sample in audioData)
        {
            energy += sample * sample;
        }
        return energy / audioData.Length;
    }

    private void AdjustGain(ref float energy){
        if (energy * _gain > 1){
            _gain = 1 / energy;
        }

        _gain *= GainIncreaseFactor;
        energy *= _gain;
    }

    private bool IsBeat(float instantEnergy){
        float averageEnergy = CalculateAverageEnergy();
        return instantEnergy > SensitivityConstant * averageEnergy;
    }

    private float CalculateAverageEnergy(){
        float sum = _energyHistory.Sum();
        return sum / _energyHistory.Length;
    }

    private void UpdateEnergyHistory(float instantEnergy){
        _energyHistory[_energyHistoryIndex] = instantEnergy;
        _energyHistoryIndex = (_energyHistoryIndex + 1) % _energyHistory.Length;
    }
}


public interface IBeatDetector{
    event Action OnBeatDetected;

    void ProcessData(float[] audioData);
}
