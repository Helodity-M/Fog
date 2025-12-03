using TMPro;
using UnityEngine;

public class NoteVFX : MonoBehaviour
{
    [SerializeField] NoteAccuracyValue<string> Text;
    [SerializeField] NoteAccuracyValue<ParticleSystem> HitParticles;

    [SerializeField] float VelocityMagnitude;
    [SerializeField] float RotationMagnitude;

    [SerializeField] NoteAccuracyValue<Gradient> TextColor;
    [SerializeField] TMP_Text Textbox;
    [SerializeField] float LifeSpan;
    float lifeSpanR = -10;
    NoteAccuracy playedAccuracy;

    private void Update()
    {
        if(lifeSpanR > -1)
        {
            lifeSpanR -= Time.deltaTime;
            Textbox.color = TextColor.GetValue(playedAccuracy).Evaluate(1 - lifeSpanR / LifeSpan);
            
            if(lifeSpanR < 0)
                Destroy(gameObject);

        }
    }
    public void PlayVFX(NoteAccuracy accuracy)
    {
        playedAccuracy = accuracy;
        lifeSpanR = LifeSpan;
        float rng = Random.Range(-1.0f, 1.0f);
        Vector2 velocity = new Vector2(rng, 3).normalized * VelocityMagnitude;
        GetComponent<Rigidbody2D>().linearVelocity = velocity;
        GetComponent<Rigidbody2D>().angularVelocity = rng * RotationMagnitude;
        Textbox.text = Text.GetValue(accuracy);
        HitParticles.GetValue(accuracy).Play();
    }

}
