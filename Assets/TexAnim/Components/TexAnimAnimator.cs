using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using TexAnim.Components
;


#if UNITY_EDITOR
using UnityEditor;
#endif

using static ScriptableObject_AnimationsBank;

namespace TexAnim.Components
{
    public class TexAnimAnimator : MonoBehaviour
    {

        [Header("Base Fields")]
        [SerializeField] private bool _playOnStart;
        [SerializeField] private ScriptableObject_AnimationsBank _animationsBank;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private AnimationsTypes _currentAnimation;
        [SerializeField] private AnimationsTypes _baseAnimation;
        private Material _animatedMaterialInstance;



        [Header("Transition Settings")]
        [SerializeField, Range(0, 2.5f)] private float _maxTransitionTime;
        private float _currentAnimationTime;
        private float _nextAnimationTime;

        [Range(0, 1)] private float _currentBlendValue;
        private bool _isTransitionning;
        private bool _loopAnimation;
        private bool _isTrigger;
        private bool _canAnimate = true;

        [Space(10)]
        [Header("Debug Parameter")]
        [SerializeField, Range(0, 2.0f)] private float debugSlowingAnimation = 1;

        [Space(10)]
        [SerializeField] private AnimationsTypes _targetedAnimations;
        private float lastFrameTime;



        #region Getters & Setters
        public AnimationsTypes CurrentAnimation { get => _currentAnimation; }
        public ScriptableObject_AnimationsBank AnimationsBank { get => _animationsBank; set => _animationsBank = value; }
        public AnimationsTypes TargetedAnimations { get => _targetedAnimations; }
        #endregion

        private void Awake()
        {
            _animatedMaterialInstance = _meshRenderer.material;
        }

        private void Start()
        {
            if (_playOnStart)
            {
                ResetAnimator();
            }
        }

        public void ResetAnimator()
        {
            _canAnimate = true;
            _loopAnimation = true;
            _baseAnimation = AnimationsTypes.Idle;
            _currentAnimation = AnimationsTypes.Idle;

            ApplyAnimation();
        }

        // Update is called once per frame
        void Update()
        {
            #region Get Delta Time
            float deltaTime;

#if UNITY_EDITOR
            // Calculate deltaTime in the editor using EditorApplication.timeSinceStartup
            deltaTime = (float)(EditorApplication.timeSinceStartup - lastFrameTime);
#else
        // In play mode, use Time.deltaTime
        deltaTime = Time.deltaTime;
#endif

            // Your update logic using deltaTime
            lastFrameTime = (float)EditorApplication.timeSinceStartup;
            #endregion

            //Don't change the order functions !
            if (_canAnimate)
                HandleAnimationsTimers(deltaTime);

            if (_isTransitionning)
                HandleBlendingTransition(deltaTime);

            if (_isTrigger && !_isTransitionning && _currentAnimationTime >= _animationsBank.GetAnimationLength(_currentAnimation))
                HandleTrigger();

            SetAnimationsTimers();
        }


        private void HandleTrigger()
        {
            if (!_loopAnimation)
            {
                _canAnimate = false;
                _currentAnimationTime = 0;
                _isTrigger = false;
            }
            else
            {
                /*Debug.Log("End of Triggered Animation");
                _currentAnimationTime = _nextAnimationTime;
                _currentAnimation = _baseAnimation;
                ApplyAnimation();
                _isTrigger = false;*/

                Debug.Log("End of Triggered Animation");
                _currentAnimationTime = _nextAnimationTime;
                _currentAnimation = _baseAnimation;
                Play(_currentAnimation);
                _isTrigger = false;
            }
        }

        private void HandleBlendingTransition(float deltaTime)
        {
            float blendIncrementValue = ((1 / _maxTransitionTime) * deltaTime) * debugSlowingAnimation;
            _animatedMaterialInstance.SetFloat("_Blend", _currentBlendValue + blendIncrementValue);

            //To keep count of what's the current value of the blending without having to get it from the material.
            _currentBlendValue += blendIncrementValue;

            if (_currentBlendValue >= 1.0f)
            {
                Debug.Log("EndBlend");
                _currentBlendValue = 0;
                _currentAnimationTime = _nextAnimationTime;
                ApplyAnimation();
            }
        }

        private void HandleAnimationsTimers(float deltaTime)
        {
            _currentAnimationTime += deltaTime * debugSlowingAnimation;

            _nextAnimationTime += deltaTime * debugSlowingAnimation;
        }

        private void SetAnimationsTimers()
        {
            if (_currentAnimationTime >= _animatedMaterialInstance.GetFloat("_CurrentAnimLen"))
            {

                if (!_loopAnimation && !_isTransitionning)
                {
                    _canAnimate = false;
                }

                _currentAnimationTime = 0.0f;
            }
            _animatedMaterialInstance.SetFloat("_CurrentAnimationTime", _currentAnimationTime / _animationsBank.GetAnimationLength(_currentAnimation));


            if (_nextAnimationTime >= _animatedMaterialInstance.GetFloat("_NextAnimLen"))
            {
                _nextAnimationTime = 0.0f;
            }
            _animatedMaterialInstance.SetFloat("_NextAnimationTime", _nextAnimationTime / _animatedMaterialInstance.GetFloat("_NextAnimLen"));
        }




        public void StartTrigger(AnimationsTypes animationType)
        {
            if (CheckCurrentAnimIsLooping()) return;
            CheckNextAnimation();

            if (_baseAnimation != _currentAnimation && _currentAnimation != animationType)
            {
                _baseAnimation = _currentAnimation;
            }

            _currentAnimation = animationType;


            //Setting Transition Animation Values.
            _animatedMaterialInstance.SetTexture("_NextAnimMap", _animationsBank.GetAnimation(animationType));
            _animatedMaterialInstance.SetFloat("_NextAnimLen", _animationsBank.GetAnimationLength(animationType));
            SetIsTransitionning(true);
            _isTrigger = true;
        }


        public void Play(AnimationsTypes animationType)
        {
            if (CheckCurrentAnimIsLooping()) return;
            CheckNextAnimation();

            _loopAnimation = true;
            _currentAnimation = animationType;
            _baseAnimation = animationType;

            //Setting Transition Animation Values.
            _animatedMaterialInstance.SetTexture("_NextAnimMap", _animationsBank.GetAnimation(animationType));
            _animatedMaterialInstance.SetFloat("_NextAnimLen", _animationsBank.GetAnimationLength(animationType));
            SetIsTransitionning(true);
        }

        public void Play(AnimationsTypes animationType, bool loopAnimation)
        {
            if (CheckCurrentAnimIsLooping()) return;
            CheckNextAnimation();

            _loopAnimation = loopAnimation;
            _currentAnimation = animationType;
            _baseAnimation = animationType;

            //Setting Transition Animation Values.
            _animatedMaterialInstance.SetTexture("_NextAnimMap", _animationsBank.GetAnimation(animationType));
            _animatedMaterialInstance.SetFloat("_NextAnimLen", _animationsBank.GetAnimationLength(animationType));
            SetIsTransitionning(true);
        }


        private void ApplyAnimation()
        {
            //Set Current Animation Values
            _animatedMaterialInstance.SetTexture("_CurrentAnimMap", _animationsBank.GetAnimation(_currentAnimation));
            _animatedMaterialInstance.SetFloat("_CurrentAnimLen", _animationsBank.GetAnimationLength(_currentAnimation));

            //Reset blending
            _animatedMaterialInstance.SetFloat("_Blend", 0);

            SetIsTransitionning(false);
        }

        private void CheckNextAnimation()
        {
            _nextAnimationTime = 0;
        }


        private bool CheckCurrentAnimIsLooping()
        {
            if (!_loopAnimation) return true;
            else return false;
        }


        private void SetIsTransitionning(bool value)
        {
            _isTransitionning = value;
        }
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(TexAnimAnimator))]
public class TexAnim_AnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var customAnimator = (TexAnimAnimator)target;
        if (customAnimator == null) return;

        DrawDefaultInspector();

        Undo.RecordObject(customAnimator, "Change CustomAnimator");

        if (GUILayout.Button("Play Target Animation"))
        {
            customAnimator.Play(customAnimator.TargetedAnimations);
        }

        if (GUILayout.Button("Trigger Target Animation"))
        {
            customAnimator.StartTrigger(customAnimator.TargetedAnimations);
        }


        if (GUILayout.Button("Play Target Animation And Stop"))
        {
            customAnimator.Play(customAnimator.TargetedAnimations, false);
        }

        if (GUILayout.Button("Reset"))
        {
            customAnimator.ResetAnimator();
        }
    }

}
#endif