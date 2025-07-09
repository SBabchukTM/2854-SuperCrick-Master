using UnityEngine;

namespace Application.UI
{
    public class ChooseTeamButton : SimpleButton
    {
        [SerializeField] private GameObject _mark;

        public void EnableMark(bool hasMark) =>
                _mark.gameObject.SetActive(hasMark);
    }
}