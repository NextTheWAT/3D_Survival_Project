public interface IInteractable
{
    /// <summary>
    /// format string data for UI prompt
    /// </summary>
    string GetInteractPrompt();
    /// <summary>
    /// Called when the player interacts with this resource object.
    /// </summary>
    void OnInteract();

}
