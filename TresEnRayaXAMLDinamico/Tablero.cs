using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Linq;
using Microsoft.Expression.Interactivity.Media;
using System.Windows.Interactivity;

namespace TresEnRaya.TresEnRayaXAMLDinamico
{
    public class Tablero : Grid, INotifyPropertyChanged
    {
        #region Eventos
            public event PropertyChangedEventHandler PropertyChanged; 
        #endregion

        #region Variables Privadas
            int Filas = 3;
            int Columnas = 3;
            private int[] posicionesTablero;
            bool turnoActual = true; 
        #endregion

        #region Propiedad Privadas
        /// <summary>
        /// Turno Actual
        /// </summary>
        private bool TurnoActual
        {
            get
            {
                return turnoActual;
            }
            set
            {
                turnoActual = value;
                SetPropiedad("TurnoActual");
            }
        }
        #endregion

        #region Propiedades Públicas
       
        /// <summary>
        /// Obtiene el nombre del Jugador Actual
        /// </summary>
        public string Jugador
        {
            get { return turnoActual ? "Jugador 1" : "Jugador 2"; }
        }

        /// <summary>
        /// Obtiene el color del jugador actual
        /// </summary>
        public SolidColorBrush ColorJugador
        {
            get { return new SolidColorBrush(turnoActual ? Colors.Blue : Colors.Red); }
        }

        #endregion

        #region Constructor SIN PARÁMETROS
        public Tablero()
        {
            CrearTablero();
        } 
        #endregion

        #region Métodos Privados
        private void SetPropiedad(string propiedad)
        {
            if (PropertyChanged != null)
            {
                if (propiedad == "TurnoActual")
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Jugador"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ColorJugador"));
                }

                PropertyChanged(this, new PropertyChangedEventArgs(propiedad));
            }
        }

        private void CrearTablero()
        {
            posicionesTablero = new int[Filas * Columnas];
            this.Children.Clear();
            this.RowDefinitions.Clear();
            this.ColumnDefinitions.Clear();
            this.ShowGridLines = true;
            TurnoActual = true;

            #region Primer paso crear Filas y Columnas del Grid
            for (int fila = 0; fila < Filas; fila++)
            {
                this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int columna = 0; columna < Columnas; columna++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }
            #endregion

            #region Crear elementos triestado en cada celda
            for (int fila = 0; fila < Filas; fila++)
            {
                for (int columna = 0; columna < Columnas; columna++)
                {
                    Grid b = new Grid();
                    b.Name = String.Format("BotonFila{0}Columna{1}", fila, columna);
                    b.Background = new SolidColorBrush(Colors.Black);
                    b.Tap += new EventHandler<GestureEventArgs>(b_Tap);
                    b.Tag = Filas * fila + columna;
                    SetRow(b, fila);
                    SetColumn(b, columna);
                    this.Children.Add(b);
                }
            }
            #endregion
        }

        /// <summary>
        /// Método encargado de agregar el control correspondiente al grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void b_Tap(object sender, GestureEventArgs e)
        {
            Grid tb = sender as Grid;
            if (TurnoActual)
            {
                //tb.Children.Add(new Cruz());
                tb.Children.Add(GenerarControlCruz());
            }
            else
                tb.Children.Add(new Circulo());

            posicionesTablero[(int)tb.Tag] = TurnoActual ? 1 : 2;
            TurnoActual = !TurnoActual;
            tb.Tap -= b_Tap;

            if (FinDelJuego())
            {
                ((tb.Children[0] as UserControl).Resources["Storyboard1"] as Storyboard).Completed += new EventHandler(Tablero_Completed);
            }
        }

        private UIElement GenerarControlCruz()
        {
            UserControl cruz = new UserControl();
            //<Grid x:Name="LayoutRoot">
            Grid layoutRoot = new Grid() { Name = "LayoutRoot" };   
            //    <Grid x:Name="grid" Margin="-25,-4,72,58" RenderTransformOrigin="0.5,0.5">
            Grid grid = new Grid() { Name = "grid", Margin = new Thickness(-25, -4, 72, 58), RenderTransformOrigin = new Point(0.5, 0.5) };
            //        <Grid.RenderTransform>
            //            <CompositeTransform/>
            //        </Grid.RenderTransform>
            var triggers = Interaction.GetTriggers(grid);
            System.Windows.Interactivity.EventTrigger trigger = new System.Windows.Interactivity.EventTrigger("Load
            triggers.Add(new ControlStoryboardAction() { Storyboard = GetStoryboardCruz() });
            //        <i:Interaction.Triggers>
            //            <i:EventTrigger>
            //                <eim:ControlStoryboardAction Storyboard="{StaticResource Storyboard1}"/>
            //            </i:EventTrigger>
            //        </i:Interaction.Triggers>
            //        <Path x:Name="path1" Data="M0,0 L98,102" UseLayoutRounding="False" StrokeThickness="6">
            //            <Path.Stroke>
            //                <RadialGradientBrush>
            //                    <GradientStop Color="#FF161381" Offset="0"/>
            //                    <GradientStop Color="#FF3D3AB2" Offset="0.997"/>
            //                    <GradientStop Color="#FF3631DC" Offset="0.488"/>
            //                </RadialGradientBrush>
            //            </Path.Stroke>
            //        </Path>
            //        <Path x:Name="path" Data="M0,0 L98,102" UseLayoutRounding="False" RenderTransformOrigin="0.5,0.5" StrokeThickness="6">
            //            <Path.Stroke>
            //                <RadialGradientBrush>
            //                    <GradientStop Color="#FF161381" Offset="0"/>
            //                    <GradientStop Color="#FF3D3AB2" Offset="0.997"/>
            //                    <GradientStop Color="#FF3631DC" Offset="0.488"/>
            //                </RadialGradientBrush>
            //            </Path.Stroke>
            //            <Path.RenderTransform>
            //                <CompositeTransform/>
            //            </Path.RenderTransform>
            //        </Path>
            //    </Grid>
            //</Grid>
            
            return cruz;
        }

        Storyboard GetStoryboardCruz()
        {
 	        throw new NotImplementedException();
        } 

        private void Tablero_Completed(object sender, EventArgs e)
        {
            int ganador = GetGanador();
            if (
                MessageBox.Show(
                    String.Concat(
                        "El juego ha terminado.\n",
                        ganador == 0 ? "No existen combinaciones" : String.Format("Ha ganado el jugador: {0}", ganador),
                        "\n¿Desea jugar otra partida?"),
                    "Fin",
                    MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                CrearTablero();
            }
        }

        private bool FinDelJuego()
        {
            return EstaLlenoElTablero() || GetGanador() != 0;
        }

        private bool EstaLlenoElTablero()
        {
            return !posicionesTablero.Contains(0);
        }

        private int GetGanador()
        {
            int[,] combinacionesGanadoras = new int[8, 3] { {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, 
                                                       {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, 
                                                       {0, 4, 8}, {2, 4, 6} };

            for (int i = 0; i < 8; i++)
            {
                if (posicionesTablero[combinacionesGanadoras[i, 0]] == posicionesTablero[combinacionesGanadoras[i, 1]] &&
                    posicionesTablero[combinacionesGanadoras[i, 1]] == posicionesTablero[combinacionesGanadoras[i, 2]] &&
                    posicionesTablero[combinacionesGanadoras[i, 0]] != 0)
                {
                    return posicionesTablero[combinacionesGanadoras[i, 0]];
                }
            }

            return 0;
        } 
        #endregion
    }
}
