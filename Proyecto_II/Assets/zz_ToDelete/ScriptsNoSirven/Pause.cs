using System;

/* NOMBRE CLASE: Pause
 * AUTOR: Jone Sainz Egea
 * FECHA: 13/03/2025
 * DESCRIPCIÓN: Clase que gestiona el evento de pausa
 * VERSIÓN: 1.0 estructura básica
 */
public static class Pause
{
    // Eventos para delegar notificaciones de que algo ha sucedido
    public static event Action OnPause; // Action es un método que no toma parámetros y no devuelve un valor
    public static event Action OnResume;

    // Método para activar el evento OnPause
    public static void TriggerPause() => OnPause?.Invoke(); // Invoca el evento solo si no es null

    // Método para activar el evento OnResume 
    public static void TriggerResume() => OnResume?.Invoke(); // Invoca el evento solo si no es null
}
