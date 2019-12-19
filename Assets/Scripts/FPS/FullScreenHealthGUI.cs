using UnityEngine;
using UnityEngine.UI;

public class FullScreenHealthGUI : MonoBehaviour {

    public Texture2D[] screenBloodTextures;                     //container array of the blood screen textures
    public Image hitImage;
    public float flashSpeed = 5f;                               //The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     //The colour the damageImage is set to, to flash.
    public int drawDepth;
    private float alpha;                                          //Transparency to apply to screen textures  

    public CharacterHealth characterHealth;


    private void hurtFlashing() {
        // If the player has just been damaged...
        if (characterHealth.isHurting && !characterHealth.isDead) {
            // ... set the colour of the damageImage to the flash colour.
            hitImage.color = flashColour;
        }
        // Otherwise...
        else {
            // ... transition the colour back to clear.
            hitImage.color = Color.Lerp(hitImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

    void OnGUI() {

        if (characterHealth.isDead) {
            return;
        }

        hurtFlashing();

        //get the corresponding texture array index
        float percentagePerImage = 100 / screenBloodTextures.Length;
        float takenHealthPercentage = 100 - ((characterHealth.currentHealth * 100) / characterHealth.maxHealth);
        int textureIndex = (int)(takenHealthPercentage / percentagePerImage);

        if (textureIndex == screenBloodTextures.Length)
            textureIndex--;

        //adjusting alpha based on player's left health percetage
        alpha = (1 - (characterHealth.currentHealth * 100 / characterHealth.maxHealth) / 100);

        //drawing the texture
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screenBloodTextures[textureIndex]);
    }
}
