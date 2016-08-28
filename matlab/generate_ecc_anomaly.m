% Generates a table of eccentric anomalies for all possible
% mean anomalies (-2*pi to 2*pi)
function [mean_anomalies, ecc_anomalies] = generate_ecc_anomaly(eccentricity, steps)
	mean_anomalies = linspace(0, 2*pi, steps);

    ecc_anomalies = [];
    
    for mean_anomaly_id = 1:steps,
        mean_anomaly = mean_anomalies(mean_anomaly_id);
        
        syms ecc_anomaly;
        ecc_anomalies = [ecc_anomalies, vpasolve(mean_anomaly == ecc_anomaly - eccentricity * sin(ecc_anomaly), ecc_anomaly)];
    end
end