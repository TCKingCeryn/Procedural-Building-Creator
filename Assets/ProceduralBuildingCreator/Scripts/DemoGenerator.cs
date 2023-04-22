using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ModularBuildingsFramework.Demo
{
	public class DemoGenerator : MonoBehaviour
	{

        public Camera sceneCamera;
        public Transform buildingHolder;

        [SerializeField]
        internal List<Object> builders;

        internal int _builderIndex;
        internal float _draftLength;
        internal float _draftHeight;
        internal float _draftDepth;
        internal bool _isRunDecorators;

        internal bool isDraftDirty;             

        public int builderIndex
        {
            get { return _builderIndex; }
            set
            {
                _builderIndex = value;
                isDraftDirty = true;
            }
        }
        public float draftLength
        {
            get { return _draftLength; }
            set
            {
                _draftLength = value;
                isDraftDirty = true;
            }
        }
        public float draftHeight
        {
            get { return _draftHeight; }
            set
            {
                _draftHeight = value;
                isDraftDirty = true;
            }
        }
        public float draftDepth
        {
            get { return _draftDepth; }
            set
            {
                _draftDepth = value;
                isDraftDirty = true;
            }
        }
        public bool isRunDecorators
        {
            get { return _isRunDecorators; }
            set
            {
                _isRunDecorators = value;
                isDraftDirty = true;
            }
        }



        void Awake()
        {
            if(!sceneCamera) sceneCamera = Camera.main;
            buildingHolder = new GameObject("BuildingHolder").transform;

            draftLength = 20f;
            draftHeight = 20f;
            draftDepth = 20f;

            isRunDecorators = true;
            //sceneCamera.enabled = false;
        }
        IEnumerator Start()
        {
            StartCoroutine(DoRebuild());

            yield return new WaitForSeconds(1f);
            //sceneCamera.enabled = true;
        }


        public void RequestRebuild()
        {
            isDraftDirty = true;
        }
        IEnumerator DoRebuild()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                if (isDraftDirty)
                {
                    Utils.DeleteChilds(buildingHolder);
                    UpdateTargetPosition();

                    var draft = CreateDraft();
                    var builder = GetBuilder();
                    builder.Build(draft);

                    if (isRunDecorators)
                    {
                        Utils.RunDecorators(buildingHolder);
                    }

                    isDraftDirty = false;
                }
            }
        }

        void UpdateTargetPosition()
        {
            buildingHolder.position = Vector3.zero - buildingHolder.right * draftLength * 0.5f - buildingHolder.forward * draftDepth * 0.5f;
        }


        public BoxDraft CreateDraft()
        {
            var result = BoxDraft.Create();
            result.Parse(buildingHolder);

            result.length = draftLength;
            result.height = draftHeight;
            result.depth = draftDepth;

            return result;
        }
        public List<Object> GetBuilders()
        {
            return builders;
        }
        IBoxBuilder GetBuilder()
        {
            return builders[builderIndex] as IBoxBuilder;
        }
        public DemoGenerator()
        {
            builders = new List<Object>();
        }
    

    }
}