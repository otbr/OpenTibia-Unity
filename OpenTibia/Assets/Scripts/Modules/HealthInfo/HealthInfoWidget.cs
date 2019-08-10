﻿using OpenTibiaUnity.Core.Creatures;
using UnityEngine;
using UnityEngine.UI;

namespace OpenTibiaUnity.Modules.HealthInfo
{
    public class HealthInfoWidget : Core.Components.Base.AbstractComponent
    {
        public const int BarWidth = 94;
        
        [SerializeField] private RawImage m_HealthBarImageComponent = null;
        [SerializeField] private RawImage m_ManaBarImageComponent = null;

        [SerializeField] private TMPro.TextMeshProUGUI m_HealthValueText = null;
        [SerializeField] private TMPro.TextMeshProUGUI m_ManaValueText = null;

        protected override void Awake() {
            base.Awake();
            Creature.onSkillChange.AddListener(OnSkillChange);
        }

        private void OnSkillChange(Creature creature, SkillType skillType, Skill skill) {
            var player = creature as Player;
            if (!player || (skillType != SkillType.Health && skillType != SkillType.Mana))
                return;

            RawImage imageComponent;
            TMPro.TextMeshProUGUI textComponent;
            if (skillType == SkillType.Health) {
                imageComponent = m_HealthBarImageComponent;
                textComponent = m_HealthValueText;
            } else {
                imageComponent = m_ManaBarImageComponent;
                textComponent = m_ManaValueText;
            }

            var rectTransform = imageComponent.GetComponent<RectTransform>();

            // setting new width
            var percent = skill.Level / (float)skill.BaseLevel;
            var rect = new Rect(imageComponent.uvRect);
            rect.width = percent;
            imageComponent.uvRect = rect;

            rect = new Rect(rectTransform.rect);
            rect.width = BarWidth * percent;
            rectTransform.sizeDelta = new Vector2(BarWidth * percent, rectTransform.sizeDelta.y);

            // setting text
            textComponent.text = skill.Level.ToString();
        }
    }
}
