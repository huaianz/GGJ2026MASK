using UnityEngine;

// 可交互接口
public interface IInteractable
{
    void OnInteract(GameObject player);
    void ShowHint();
    void HideHint();
    bool CanInteract();

    // 可交互类型枚举
    public enum InteractableType
    {
        AlarmClock,
        Refrigerator,
        Stove,
        Window,
        Book,
        Child,
        Robot,
        Clothes,
        Wardrobe,
        Desk
    }
}
