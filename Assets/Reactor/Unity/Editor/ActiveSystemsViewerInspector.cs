using System.Linq;
using Reactor.Extensions;
using Reactor.Systems;
using Reactor.Systems.Executor;
using UnityEditor;
using UnityEngine;

namespace Reactor.Unity.Helpers
{
    [CustomEditor(typeof(ActiveSystemsViewer))]
    public class ActiveSystemsViewerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var activeSystemsViewer = (ActiveSystemsViewer)target;
            var executor = activeSystemsViewer.SystemExecutor;

            if (executor == null)
            {
                EditorGUILayout.LabelField("System Executor Inactive");
                return;
            }

            var isNormalExecutorType = executor is SystemExecutor;
            var typedExecutor = executor as SystemExecutor;

            EditorGUILayout.TextField("Setup Systems");
            EditorGUILayout.Space();
            foreach (var system in executor.Systems.OfType<ISetupSystem>())
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("System: " + system.GetType().Name);
                if(isNormalExecutorType)
                {  EditorGUILayout.LabelField("Active Subscriptions: " + typedExecutor.GetSubscriptionCountForSystem(system)); }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.TextField("Group Systems");
            EditorGUILayout.Space();
            foreach (var system in executor.Systems.OfType<IGroupReactionSystem>())
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("System: " + system.GetType().Name);
                if (isNormalExecutorType)
                { EditorGUILayout.LabelField("Active Subscriptions: " + typedExecutor.GetSubscriptionCountForSystem(system)); }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.TextField("Entity Systems");
            EditorGUILayout.Space();
            foreach (var system in executor.Systems.OfType<IEntityReactionSystem>())
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("System: " + system.GetType().Name);
                if (isNormalExecutorType)
                { EditorGUILayout.LabelField("Active Subscriptions: " + typedExecutor.GetSubscriptionCountForSystem(system)); }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.TextField("Entity to Entity Systems");
            EditorGUILayout.Space();
            foreach (var system in executor.Systems.OfType<IInteractReactionSystem>())
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("System: " + system.GetType().Name);
                if (isNormalExecutorType)
                { EditorGUILayout.LabelField("Active Subscriptions: " + typedExecutor.GetSubscriptionCountForSystem(system)); }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            EditorGUILayout.TextField("Total Subscriptions Across All Systems: " + typedExecutor.GetTotalSubscriptions());
        }
    }
}