using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIControl : MonoBehaviour
{
    public void HandleGameLoad() {
        // TODO: Display game start overlay
        return;
    }

    public void HandleGameStart() {
        // TODO: Clear Game start overlay
        return;
    }

    public void HandleHoverDie(int lastSelectedDie, int selectedDie) {
        // TODO: Move selection arrow
        // TODO: Make old highlighted tiles invisble
        // TODO: Make new highlighted tiles visble
        return;
    } 

    public void HandleSelectDie(int selectedDie) {
        // TODO: Increase shadow alpha
        // TODO: make selection arrow invisible
        // TODO: Special highlight for currently selected tile
        return;
    }

    public void HandleDeselectDie(int selectedDie) {
        // TODO: Decrease shadow alpha
        // TODO: Make selection arrow visible
        // TODO: remomve special highlight for selected tile
        return;
    }

    public void HandleHoverTile(int lastSelectedTile, int selectedTile) {
        // TODO: replace special highlight on old tile
        // TODO: replace standard highlight on new tile
        return;
    }

    public void HandleSelectTile(int selectedTile) {
        // TODO: remove all tile highlights
        // TODO: Poof animation and teleport character
        return;
    }
}
