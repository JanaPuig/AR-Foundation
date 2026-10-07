using UnityEngine;


// Reproduce las canciones de la lista una detrás de otra y vuelve a empezar





[RequireComponent(typeof(AudioSource))]
public class MusicPlaylist : MonoBehaviour
{
    [SerializeField] private AudioClip[] canciones;
    [SerializeField, Range(0f, 1f)] private float volumen = 0.5f;

    private AudioSource fuente;
    private int indice = -1;

    private void Awake()
    {
        fuente = GetComponent<AudioSource>();
        fuente.playOnAwake = false;
        fuente.loop = false;        // el bucle lo lleva este script, no el AudioSource
        fuente.spatialBlend = 0f;   // sonido 2D: se oye igual estés donde estés
    }

    private void Start()
    {
        if (canciones == null || canciones.Length == 0)
        {
            Debug.LogWarning("MusicPlaylist: no hay canciones asignadas en el Inspector.");
            enabled = false;
            return;
        }

        ReproducirSiguiente();
    }

    private void Update()
    {
        fuente.volume = volumen;

        // Al terminar una canción, el AudioSource se para y su tiempo vuelve a 0.
        // (Si la app solo está en pausa, el tiempo no es 0 y no se salta la canción.)
        if (!fuente.isPlaying && fuente.time == 0f)
        {
            ReproducirSiguiente();
        }
    }

    private void ReproducirSiguiente()
    {
        indice = (indice + 1) % canciones.Length;

        if (canciones[indice] == null) return; // hueco vacío en la lista: se salta

        fuente.clip = canciones[indice];
        fuente.Play();
    }
}
