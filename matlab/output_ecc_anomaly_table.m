function output_ecc_anomaly_table(eccentricity, steps)
	[mean_anomalies, ecc_anomalies] = generate_ecc_anomaly(eccentricity, steps);
	csv_filename = strcat('mean_to_ecc_anom_', num2str(eccentricity), '.csv');
	dlmwrite(csv_filename, [mean_anomalies', ecc_anomalies'], 'delimiter', ',', 'precision', 9); 
end