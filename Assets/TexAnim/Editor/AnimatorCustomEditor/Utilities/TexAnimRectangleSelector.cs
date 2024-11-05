using System;
using System.Collections.Generic;
using System.Linq;
using TexAnim.Windows;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEditor.Experimental.GraphView
{
    //
    // Summary:
    //     Rectangle selection box manipulator.
    public class TexAnimRectangleSelector : MouseManipulator
    {
        private class RectangleSelect : ImmediateModeElement
        {
            public Vector2 start { get; set; }

            public Vector2 end { get; set; }

            protected override void ImmediateRepaint()
            {
                VisualElement visualElement = base.parent;
                Vector2 vector = start;
                Vector2 vector2 = end;
                if (!(start == end))
                {
                    vector += visualElement.layout.position;
                    vector2 += visualElement.layout.position;
                    Rect rect = default(Rect);
                    rect.min = new Vector2(Math.Min(vector.x, vector2.x), Math.Min(vector.y, vector2.y));
                    rect.max = new Vector2(Math.Max(vector.x, vector2.x), Math.Max(vector.y, vector2.y));
                    Rect rect2 = rect;
                    Color col = new Color(1f, 0.6f, 0f, 1f);
                    float segmentsLength = 5f;
                    Vector3[] array = new Vector3[4]
                    {
                        new Vector3(rect2.xMin, rect2.yMin, 0f),
                        new Vector3(rect2.xMax, rect2.yMin, 0f),
                        new Vector3(rect2.xMax, rect2.yMax, 0f),
                        new Vector3(rect2.xMin, rect2.yMax, 0f)
                    };
                    DrawDottedLine(array[0], array[1], segmentsLength, col);
                    DrawDottedLine(array[1], array[2], segmentsLength, col);
                    DrawDottedLine(array[2], array[3], segmentsLength, col);
                    DrawDottedLine(array[3], array[0], segmentsLength, col);
                }
            }

            private void DrawDottedLine(Vector3 p1, Vector3 p2, float segmentsLength, Color col)
            {
                /*HandleUtility.ApplyWireMaterial();
                GL.Begin(1);
                GL.Color(col);
                float num = Vector3.Distance(p1, p2);
                int num2 = Mathf.CeilToInt(num / segmentsLength);
                for (int i = 0; i < num2; i += 2)
                {
                    GL.Vertex(Vector3.Lerp(p1, p2, (float)i * segmentsLength / num));
                    GL.Vertex(Vector3.Lerp(p1, p2, (float)(i + 1) * segmentsLength / num));
                }

                GL.End();*/

                /*VisualElement rectangle = new VisualElement();
                TransformOrigin newOrigin = new TransformOrigin();
                newOrigin.x = 0;
                newOrigin.y = 0;
                newOrigin.z = 0;

                rectangle.style.transformOrigin = newOrigin;


                Position newPosition = new Position(p1);
                rectangle.style.position = new Position();
                rectangle.style.right = */
            }
        }

        private readonly RectangleSelect m_Rectangle;

        private bool m_Active;

        private TexAnimAnimatorEditorWindow m_EditorWindow;


        //
        // Summary:
        //     RectangleSelector's constructor.
        public TexAnimRectangleSelector(TexAnimAnimatorEditorWindow editorWindow)
        {

            base.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse
            });
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
            {
                base.activators.Add(new ManipulatorActivationFilter
                {
                    button = MouseButton.LeftMouse,
                    modifiers = EventModifiers.Command
                });
            }
            else
            {
                base.activators.Add(new ManipulatorActivationFilter
                {
                    button = MouseButton.LeftMouse,
                    modifiers = EventModifiers.Control
                });
            }

            m_EditorWindow = editorWindow;
            m_Rectangle = new RectangleSelect();
            m_Rectangle.style.position = Position.Absolute;
            m_Rectangle.style.top = 0f;
            m_Rectangle.style.left = 0f;
            m_Rectangle.style.bottom = 0f;
            m_Rectangle.style.right = 0f;
            m_Active = false;
        }

        //
        // Summary:
        //     Computer the axis-aligned bound rectangle.
        //
        // Parameters:
        //   position:
        //     Rectangle to bound.
        //
        //   transform:
        //     Transform.
        //
        // Returns:
        //     The axis-aligned bound.
        public Rect ComputeAxisAlignedBound(Rect position, Matrix4x4 transform)
        {
            Vector3 vector = transform.MultiplyPoint3x4(position.min);
            Vector3 vector2 = transform.MultiplyPoint3x4(position.max);
            return Rect.MinMaxRect(Math.Min(vector.x, vector2.x), Math.Min(vector.y, vector2.y), Math.Max(vector.x, vector2.x), Math.Max(vector.y, vector2.y));
        }

        //
        // Summary:
        //     Called to register click event callbacks on the target element.
        protected override void RegisterCallbacksOnTarget()
        {
            GraphView graphView = base.target as GraphView;
            if (graphView == null)
            {
                throw new InvalidOperationException("Manipulator can only be added to a GraphView");
            }

            base.target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            base.target.RegisterCallback<MouseUpEvent>(OnMouseUp);
            base.target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            base.target.RegisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOutEvent);
        }

        //
        // Summary:
        //     Called to unregister event callbacks from the target element.
        protected override void UnregisterCallbacksFromTarget()
        {
            base.target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            base.target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
            base.target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            base.target.UnregisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOutEvent);
        }

        private void OnMouseCaptureOutEvent(MouseCaptureOutEvent e)
        {
            if (m_Active)
            {
                m_Rectangle.RemoveFromHierarchy();
                m_Active = false;
            }
        }

        private void OnMouseDown(MouseDownEvent e)
        {
            if (m_Active)
            {
                e.StopImmediatePropagation();
                return;
            }

            GraphView graphView = base.target as GraphView;
            if (graphView != null && CanStartManipulation(e))
            {
                if (!e.actionKey)
                {
                    graphView.ClearSelection();
                }

                graphView.Add(m_Rectangle);
                m_Rectangle.start = e.localMousePosition - new Vector2(m_EditorWindow.parametersView.style.width.value.value, 0);
                m_Rectangle.end = m_Rectangle.start;
                m_Active = true;
                base.target.CaptureMouse();
                e.StopImmediatePropagation();
            }
        }

        private void OnMouseUp(MouseUpEvent e)
        {
            if (!m_Active)
            {
                return;
            }

            GraphView graphView = base.target as GraphView;
            if (graphView == null || !CanStopManipulation(e))
            {
                return;
            }

            graphView.Remove(m_Rectangle);
            m_Rectangle.end = e.localMousePosition - new Vector2(m_EditorWindow.parametersView.style.width.value.value, 0);
            Rect selectionRect = new Rect
            {
                min = new Vector2(Math.Min(m_Rectangle.start.x, m_Rectangle.end.x), Math.Min(m_Rectangle.start.y, m_Rectangle.end.y)),
                max = new Vector2(Math.Max(m_Rectangle.start.x, m_Rectangle.end.x), Math.Max(m_Rectangle.start.y, m_Rectangle.end.y))
            };
            selectionRect = ComputeAxisAlignedBound(selectionRect, graphView.viewTransform.matrix.inverse);
            List<ISelectable> selection = graphView.selection;
            if (!selection.Any((ISelectable ge) => ge is GraphElement && ((GraphElement)ge).IsStackable()))
            {
                List<ISelectable> newSelection = new List<ISelectable>();
                graphView.graphElements.ForEach(delegate (GraphElement child)
                {
                    Rect rectangle = graphView.contentViewContainer.ChangeCoordinatesTo(child, selectionRect);
                    if (child.IsSelectable() && child.Overlaps(rectangle) && !child.IsStackable())
                    {
                        newSelection.Add(child);
                    }
                });
                foreach (ISelectable item in newSelection)
                {
                    if (selection.Contains(item))
                    {
                        if (e.actionKey)
                        {
                            graphView.RemoveFromSelection(item);
                        }
                    }
                    else
                    {
                        graphView.AddToSelection(item);
                    }
                }
            }

            m_Active = false;
            base.target.ReleaseMouse();
            e.StopPropagation();
        }

        private void OnMouseMove(MouseMoveEvent e)
        {
            if (m_Active)
            {
                m_Rectangle.end = e.localMousePosition - new Vector2(m_EditorWindow.parametersView.style.width.value.value, 0);
                e.StopPropagation();
            }
        }
    }
}

#if false // Decompilation log
'268' items in cache
------------------
Resolve: 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\Facades\netstandard.dll'
------------------
Resolve: 'UnityEngine.SharedInternalsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.SharedInternalsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\Managed\UnityEngine\UnityEngine.SharedInternalsModule.dll'
------------------
Resolve: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll'
------------------
Resolve: 'UnityEngine.UIElementsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.UIElementsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\Managed\UnityEngine\UnityEngine.UIElementsModule.dll'
------------------
Resolve: 'UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\Managed\UnityEngine\UnityEditor.CoreModule.dll'
------------------
Resolve: 'UnityEngine.IMGUIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.IMGUIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\Managed\UnityEngine\UnityEngine.IMGUIModule.dll'
------------------
Resolve: 'UnityEngine.TextRenderingModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'UnityEngine.TextRenderingModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\Managed\UnityEngine\UnityEngine.TextRenderingModule.dll'
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\mscorlib.dll'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Core.dll'
------------------
Resolve: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.dll'
------------------
Resolve: 'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Data.dll'
------------------
Resolve: 'System.Diagnostics.Tracing, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Diagnostics.Tracing, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Drawing.dll'
------------------
Resolve: 'System.IO.Compression, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.IO.Compression, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.IO.Compression.dll'
------------------
Resolve: 'System.IO.Compression.FileSystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.IO.Compression.FileSystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.IO.Compression.FileSystem.dll'
------------------
Resolve: 'System.ComponentModel.Composition, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.ComponentModel.Composition, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.ComponentModel.Composition.dll'
------------------
Resolve: 'System.Net.Http, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Http, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Net.Http.dll'
------------------
Resolve: 'System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Numerics.dll'
------------------
Resolve: 'System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Runtime.Serialization.dll'
------------------
Resolve: 'System.Transactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Transactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Transactions.dll'
------------------
Resolve: 'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Xml.dll'
------------------
Resolve: 'System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Xml.Linq.dll'
------------------
Resolve: 'System.Data.DataSetExtensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Data.DataSetExtensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Data.DataSetExtensions.dll'
------------------
Resolve: 'System.Numerics.Vectors, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Numerics.Vectors, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.10f1\Editor\Data\UnityReferenceAssemblies\unity-4.8-api\System.Numerics.Vectors.dll'
#endif
